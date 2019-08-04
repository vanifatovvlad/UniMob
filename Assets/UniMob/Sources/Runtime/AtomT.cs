namespace UniMob
{
    // ReSharper disable once InconsistentNaming
    public interface Atom<out T>
    {
        T Value { get; }

        T Get();

        void Deactivate();

        void Invalidate();
    }
}