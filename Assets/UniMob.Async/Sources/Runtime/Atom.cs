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

        public static Atom<T> Func<T>(
            AtomPull<T> pull,
            AtomMerge<T> merge = null,
            Action onActive = null,
            Action onInactive = null,
            IEqualityComparer<T> comparer = null)
        {
            return new MutableAtom<T>(
                pull, null, merge, onActive, onInactive, comparer);
        }

        public static MutableAtom<T> Func<T>(
            AtomPull<T> pull,
            AtomPush<T> push,
            AtomMerge<T> merge = null,
            Action onActive = null,
            Action onInactive = null,
            IEqualityComparer<T> comparer = null)
        {
            return new MutableAtom<T>(pull, push, merge, onActive, onInactive, comparer);
        }

        public static ReactionAtom Reaction(Action reaction)
        {
            return new ReactionAtom(reaction);
        }

        public static void Push<T>(MutableAtom<T> atom, T value) => atom.Push(value);
    }
}