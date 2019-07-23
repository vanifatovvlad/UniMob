using System;
using System.Collections.Generic;
using System.Threading;

namespace UniMob
{
    internal sealed class TimerDispatcher : IDisposable
    {
        private static int _mainThreadId;

        private readonly Action<Exception> _exceptionHandler;
        private readonly object _lock = new object();
        private readonly List<Action> _threadedQueue = new List<Action>();
        private readonly List<DelayedCall> _threadedDelayed = new List<DelayedCall>();
        private readonly BinaryHeap<float, Action> _delayed = new BinaryHeap<float, Action>(new TimeComparer());

        private float _time;
        private bool _threadedDirty;
        private List<Action> _queue = new List<Action>();
        private List<Action> _toPass = new List<Action>();

        internal bool ThreadedDirty => _threadedDirty;

        public TimerDispatcher(int mainThreadId, Action<Exception> exceptionHandler)
        {
            _mainThreadId = mainThreadId;
            _exceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _threadedQueue.Clear();
                _threadedDelayed.Clear();
            }
            
            _delayed.Clear();
            _queue.Clear();
            _toPass.Clear();
        }

        internal void Tick(float time)
        {
            if (_threadedDirty)
            {
                lock (_lock)
                {
                    _threadedDirty = false;

                    if (_threadedQueue.Count > 0)
                    {
                        _queue.AddRange(_threadedQueue);
                        _threadedQueue.Clear();
                    }

                    if (_threadedDelayed.Count > 0)
                    {
                        foreach (var call in _threadedDelayed)
                        {
                            _delayed.Add(call.Delay, call.Action);
                        }
                    }
                }
            }

            _time = time;
            Utils.Swap(ref _queue, ref _toPass);
            _queue.Clear();

            while (_delayed.Count > 0 && _delayed.PeekKey() <= _time)
            {
                _toPass.Add(_delayed.Remove());
            }

            for (int i = 0; i < _toPass.Count; i++)
            {
                try
                {
                    _toPass[i].Invoke();
                }
                catch (Exception ex)
                {
                    _exceptionHandler(ex);
                }
            }
        }

        internal void Invoke(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (Thread.CurrentThread.ManagedThreadId == _mainThreadId)
            {
                _queue.Add(action);
            }
            else
            {
                lock (_lock)
                {
                    _threadedQueue.Add(action);
                    _threadedDirty = true;
                }
            }
        }

        internal void InvokeDelayed(float delay, Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (Thread.CurrentThread.ManagedThreadId == _mainThreadId)
            {
                _delayed.Add(_time + delay, action);
            }
            else
            {
                lock (_lock)
                {
                    _threadedDelayed.Add(new DelayedCall {Delay = delay, Action = action});
                    _threadedDirty = true;
                }
            }
        }

        private class TimeComparer : IComparer<float>
        {
            public int Compare(float x, float y)
            {
                return x.CompareTo(y);
            }
        }

        struct DelayedCall
        {
            public float Delay;
            public Action Action;
        }
    }
}