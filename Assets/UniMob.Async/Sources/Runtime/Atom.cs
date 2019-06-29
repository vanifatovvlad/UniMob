using System;
using System.Collections.Generic;

namespace UniMob.Async
{
    public static class Atom
    {
        public static MutableAtom<T> Value<T>(
            T value,
            AtomMerge<T> merge = null,
            Action onActive = null,
            Action onInactive = null,
            IEqualityComparer<T> comparer = null)
        {
            return new MutableAtom<T>(() => value, next => value = next, merge, onActive, onInactive, comparer);
        }

        public static MutableAtom<T> Computed<T>(
            AtomPull<T> pull,
            AtomPush<T> push = null,
            AtomMerge<T> merge = null,
            Action onActive = null,
            Action onInactive = null,
            IEqualityComparer<T> comparer = null)
        {
            return new MutableAtom<T>(pull, push, merge, onActive, onInactive, comparer);
        }
        
        public static MutableAtom<T> Property<T>(AtomPull<T> pull, AtomPush<T> push)
        {
            return new MutableAtom<T>(pull, push);
        }

        public static ReactionAtom CreateReaction(Action reaction)
        {
            return new ReactionAtom(reaction);
        }

        public static ReactionAtom RunReaction(Action reaction)
        {
            var atom = CreateReaction(reaction);
            atom.Get();
            return atom;
        }

        public static void Push<T>(MutableAtom<T> atom, T value) => atom.Push(value);

        public static WatchScope NoWatch => new WatchScope(null);
        
        public static AtomBase CurrentScope => AtomBase.Stack;

        public readonly struct WatchScope : IDisposable
        {
            private readonly AtomBase _parent;

            internal WatchScope(AtomBase self)
            {
                _parent = AtomBase.Stack;
                AtomBase.Stack = self;
            }

            public void Dispose()
            {
                AtomBase.Stack = _parent;
            }
        }
        
        /// <summary>
        /// Creates Future which completes when condition becomes True.
        /// If condition throw exception, Future completes with exception.
        /// </summary>
        /// <param name="cond">Watched condition</param>
        /// <returns></returns>
        public static Future<AsyncUnit> When(Func<bool> cond)
        {
            var future = new Future<AsyncUnit>();

            ReactionAtom watcher = null;
            watcher = CreateReaction(() =>
            {
                Exception exception = null;
                try
                {
                    if (!cond()) return;
                }
                catch (Exception e)
                {
                    exception = e;
                }

                using (NoWatch)
                {
                    if (exception != null) future.CompleteException(exception);
                    else future.Complete(AsyncUnit.Unit);

                    // ReSharper disable once AccessToModifiedClosure
                    watcher?.Deactivate();
                    watcher = null;
                }
            });
            
            watcher.Get();

            return future;
        }
    }
}