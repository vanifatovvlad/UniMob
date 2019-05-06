using JetBrains.Annotations;

namespace UniMob.ReView
{
    public abstract class Widget
    {
        [CanBeNull] public Key Key { get; }

        protected Widget([CanBeNull] Key key = null)
        {
            Key = key;
        }

        [NotNull]
        public abstract State CreateState();
    }
}