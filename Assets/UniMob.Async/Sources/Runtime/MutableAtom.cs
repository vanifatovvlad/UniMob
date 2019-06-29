using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniMob.Async
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

        private bool _isRunningSetter;

        public T Value
        {
            get => Get();
            set => Set(value);
        }

        public MutableAtom(
            AtomPull<T> pull,
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

        public override void Deactivate()
        {
            base.Deactivate();

            _hasCache = false;
            _cache = default;
        }

        protected override void Evaluate()
        {
            State = AtomState.Actual;

            var value = _pull();

            using (Atom.NoWatch)
            {
                if (_hasCache && _comparer.Equals(value, _cache))
                    return;

                if (_merge != null && _hasCache)
                {
                    _hasCache = true;
                    _cache = _merge(_cache, value);
                }
                else
                {
                    _hasCache = true;
                    _cache = value;
                }
            }

            ObsoleteListeners();
        }

        public T Get()
        {
            Update();
            StackPush();
            return _cache;
        }

        public void Set(T value)
        {
            if (_push == null)
                throw new InvalidOperationException("It is not possible to assign a new value to a readonly Atom");

            if (_isRunningSetter)
            {
                var message = "The setter of MutableAtom is trying to update itself. " +
                              "Did you intend to invoke Atom.Push(..), instead of the setter?";
                throw new InvalidOperationException(message);
            }

            using (Atom.NoWatch)
            {
                if (_hasCache && _comparer.Equals(value, _cache))
                    return;

                State = AtomState.Obsolete;
                _cache = default;
                _hasCache = false;

                try
                {
                    _isRunningSetter = true;
                    _push(value);
                }
                finally
                {
                    _isRunningSetter = false;
                }
            }

            ObsoleteListeners();
        }

        internal void Push(T value)
        {
            State = AtomState.Actual;
            _hasCache = true;
            _cache = value;

            ObsoleteListeners();
        }

        public override string ToString()
        {
            return _hasCache ? Convert.ToString(_cache) : "[undefined]";
        }
    }
}