namespace UniMob.Async
{
    public readonly struct Atom<T>
    {
        private readonly T _value;
        private readonly MutableAtom<T> _atom;

        public T Value => _atom != null ? _atom.Value : _value;

#if UNITY_EDITOR
        public bool DebugInitialized { get; }
#endif

        public Atom(T value)
        {
            _value = value;
            _atom = null;
#if UNITY_EDITOR
            DebugInitialized = true;
#endif
        }

        public Atom(MutableAtom<T> atom)
        {
            _value = default;
            _atom = atom;
#if UNITY_EDITOR
            DebugInitialized = true;
#endif
        }

        public T Get() => Value;

        public void Deactivate() => _atom?.Deactivate();

        public static implicit operator Atom<T>(T value) => new Atom<T>(value);
        public static implicit operator Atom<T>(MutableAtom<T> atom) => new Atom<T>(atom);
        public static implicit operator T(Atom<T> atom) => atom.Value;
    }
}