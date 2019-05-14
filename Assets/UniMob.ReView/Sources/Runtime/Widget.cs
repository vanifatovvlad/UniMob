using JetBrains.Annotations;

namespace UniMob.ReView
{
    public abstract class Widget
    {
        [CanBeNull] public Key Key { get; set; }

        [NotNull]
        public abstract State CreateState();
    }
}