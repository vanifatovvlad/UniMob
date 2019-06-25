using JetBrains.Annotations;

namespace UniMob.ReView
{
    public abstract class Widget
    {
        protected Widget([CanBeNull] Key key = null)
        {
            Key = key;
        }

        [CanBeNull] public Key Key { get; }

        [NotNull]
        public abstract State CreateState();
    }
}