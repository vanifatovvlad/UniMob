using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace UniMob
{
    public delegate T AtomPull<out T>();

    public delegate void AtomPush<in T>(T value);

    public delegate T AtomMerge<T>(T prevValue, T nextValue);

    /// <summary>
    /// Atom - is a reactive container. It computes his value by given function,
    /// watching its dependencies. If dependencies is being changed, then it updates his value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MutableAtom<T> : AtomBase, Atom<T>
    {
        private readonly IEqualityComparer<T> _comparer;
        private readonly AtomPull<T> _pull;
        private readonly AtomPush<T> _push;
        private readonly AtomMerge<T> _merge;

        private bool _hasCache;
        private T _cache;
        private Exception _cacheException;

        private bool _isRunningSetter;

        internal MutableAtom(
            [NotNull] AtomPull<T> pull,
            AtomPush<T> push = null,
            AtomMerge<T> merge = null,
            Action onActive = null,
            Action onInactive = null,
            IEqualityComparer<T> comparer = null)
            : base(onActive, onInactive)
        {
            _pull = pull ?? throw new ArgumentNullException(nameof(pull));
            _push = push;
            _merge = merge;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public T Value
        {
            get
            {
                Update();
                StackPush();

                if (_cacheException != null)
                {
                    throw _cacheException;
                }
                
                return _cache;
            }
            set
            {
                if (_push == null)
                    throw new InvalidOperationException("It is not possible to assign a new value to a readonly Atom");

                if (_isRunningSetter)
                {
                    var message = "The setter of MutableAtom is trying to update itself. " +
                                  "Did you intend to invoke Atom.Push(..), instead of the setter?";
                    throw new InvalidOperationException(message);
                }

                try
                {
                    using (Atom.NoWatch)
                    {
                        if (_hasCache && _comparer.Equals(value, _cache))
                            return;

                        State = AtomState.Obsolete;
                        _cache = default;
                        _cacheException = null;
                        _hasCache = false;

                        _isRunningSetter = true;
                        _push(value);
                    }
                }
                finally
                {
                    _isRunningSetter = false;
                    ObsoleteListeners();
                }
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();

            _hasCache = false;
            _cache = default;
            _cacheException = null;
        }

        protected override void Evaluate()
        {
            State = AtomState.Actual;

            try
            {
                var value = _pull();

                using (Atom.NoWatch)
                {
                    if (_hasCache && _comparer.Equals(value, _cache))
                        return;
                    
                    if (_merge != null && _hasCache)
                    {
                        _cache = _merge(_cache, value);
                    }
                    else
                    {
                        _cache = value;
                    }

                    _cacheException = null;
                    _hasCache = true;
                }
            }
            catch (Exception exception)
            {
                _hasCache = false;
                _cache = default;
                _cacheException = exception;
            }

            ObsoleteListeners();
        }

        public T Get() => Value;

        public void Set(T value) => Value = value;

        public void Invalidate()
        {
            State = AtomState.Obsolete;
            _hasCache = false;
            _cache = default;
            _cacheException = null;
            
            ObsoleteListeners();
        }

        internal void Push(T value)
        {
            State = AtomState.Actual;
            _hasCache = true;
            _cache = value;
            _cacheException = null;

            ObsoleteListeners();
        }

        internal void PushException(Exception exception)
        {
            State = AtomState.Actual;
            _hasCache = false;
            _cache = default;
            _cacheException = exception ?? new NullReferenceException();
            
            ObsoleteListeners();
        }

        public override string ToString()
        {
            if (_cacheException != null)
                return _cacheException.ToString();

            return _hasCache ? Convert.ToString(_cache) : "[undefined]";
        }
    }
}