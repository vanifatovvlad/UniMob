using System;
using System.Collections.Generic;

namespace UniMob.Async
{
    public abstract class AtomBase
    {
        private readonly Action _onActive;
        private readonly Action _onInactive;
        private List<AtomBase> _children;
        private List<AtomBase> _listeners;
        private bool _deactivated = true;
        
        protected AtomState State = AtomState.Obsolete;

        protected enum AtomState
        {
            Obsolete,
            Checking,
            Pulling,
            Actual,
        }
        
        public bool Deactivated => _deactivated;

        protected AtomBase(Action onActive, Action onInactive)
        {
            _onActive = onActive;
            _onInactive = onInactive;
        }

        public void Update(bool force = false)
        {
            if (State == AtomState.Pulling)
            {
                throw new Exception("Cyclic atom dependency of " + this);
            }

            if (_deactivated)
            {
                _onActive?.Invoke();
            }

            _deactivated = false;
            Actualize(force);
        }

        public virtual void Deactivate()
        {
            if (_children != null)
            {
                _children.ForEach(child => child.RemoveListener(this));
                DeleteList(ref _children);
            }

            _listeners?.ForEach(listener => listener.Check());

            if (!_deactivated)
            {
                _deactivated = true;
                _onInactive?.Invoke();
            }

            State = AtomState.Obsolete;
        }

        private void Actualize(bool force = false)
        {
            if (!force && State == AtomState.Actual)
                return;

            var parent = Stack;
            Stack = this;
            try
            {
                if (!force && State == AtomState.Checking)
                {
                    _children.ForEach(child =>
                    {
                        if (State != AtomState.Checking)
                            return;

                        child.Actualize();
                    });

                    if (State == AtomState.Checking)
                    {
                        State = AtomState.Actual;
                    }
                }

                if (force || State != AtomState.Actual)
                {
                    var oldChildren = _children;
                    if (oldChildren != null)
                    {
                        _children = null;

                        oldChildren.ForEach(child => child.RemoveListener(this));
                        DeleteList(ref oldChildren);
                    }

                    State = AtomState.Pulling;
                    Evaluate();
                }
            }
            finally
            {
                Stack = parent;
            }
        }

        protected abstract void Evaluate();

        protected void ObsoleteListeners()
        {
            _listeners?.ForEach(listener => listener.Obsolete());
        }

        private void CheckListeners()
        {
            if (_listeners != null)
            {
                _listeners.ForEach(listener => listener.Check());
            }
            else
            {
                Actualize(this);
            }
        }

        private void Check()
        {
            if (State == AtomState.Actual || State == AtomState.Pulling)
            {
                State = AtomState.Checking;
                CheckListeners();
            }
        }

        private void Obsolete()
        {
            if (State == AtomState.Obsolete)
                return;

            State = AtomState.Obsolete;
            CheckListeners();
        }

        protected void AddListener(AtomBase listener)
        {
            if (_listeners == null)
            {
                CreateList(out _listeners);
                Unreap(this);
            }

            _listeners.Add(listener);
        }

        private void RemoveListener(AtomBase listener)
        {
            if (_listeners == null)
                return;

            if (_listeners.Count == 1)
            {
                DeleteList(ref _listeners);
                Reap(this);
            }
            else
            {
                _listeners.Remove(listener);
            }
        }

        internal void AddChildren(AtomBase child)
        {
            if (_children == null)
                CreateList(out _children);

            _children.Add(child);
        }

        protected void StackPush()
        {
            var parent = Stack;
            if (parent != null)
            {
                AddListener(parent);
                parent.AddChildren(this);
            }
        }

        protected static AtomBase Stack;

        private static readonly Queue<AtomBase> Updating = new Queue<AtomBase>();
        private static readonly List<AtomBase> Reaping = new List<AtomBase>();
        private static IZone _scheduled;

        internal static void Actualize(AtomBase atom)
        {
            Updating.Enqueue(atom);
            Schedule();
        }

        private static void Reap(AtomBase atom)
        {
            Reaping.Add(atom);
            Schedule();
        }

        private static void Unreap(AtomBase atom)
        {
            Reaping.Remove(atom);
        }

        private static void Schedule()
        {
            if (_scheduled == Zone.Current)
                return;

            Zone.Current.Invoke(() =>
            {
                if (_scheduled == null)
                    return;

                _scheduled = null;

#if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample("AtomBase.Sync()");
#endif
                Sync();
#if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
#endif
            });

            _scheduled = Zone.Current;
        }

        private static void Sync()
        {
            Schedule();

            while (Updating.Count > 0)
            {
                var atom = Updating.Dequeue();

                if (Reaping.Contains(atom))
                    continue;

                if (atom.State != AtomState.Actual)
                {
                    atom.Actualize();
                }
            }

            while (Reaping.Count > 0)
            {
                var atom = Reaping[0];
                Reaping.RemoveAt(0);
                if (atom._listeners == null)
                {
                    atom.Deactivate();
                }
            }

            _scheduled = null;
        }

        private static readonly Stack<List<AtomBase>> ListPool = new Stack<List<AtomBase>>();

        private static void CreateList(out List<AtomBase> list)
        {
            list = (ListPool.Count > 0) ? ListPool.Pop() : new List<AtomBase>();
        }

        private static void DeleteList(ref List<AtomBase> list)
        {
            list.Clear();
            ListPool.Push(list);
            list = null;
        }

        internal static void Cleanup()
        {
            ListPool.Clear();
            Stack = null;
            Updating.Clear();
            Reaping.Clear();
        }
    }
}