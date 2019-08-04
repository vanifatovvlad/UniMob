using System;
using JetBrains.Annotations;

namespace UniMob.UI
{
    public abstract class Widget
    {
        private Type _type;

        protected Widget([CanBeNull] Key key = null)
        {
            Key = key;
        }

        internal Type Type => _type ?? (_type = GetType());

        [CanBeNull] public Key Key { get; }

        [NotNull]
        public abstract State CreateState();
    }
}