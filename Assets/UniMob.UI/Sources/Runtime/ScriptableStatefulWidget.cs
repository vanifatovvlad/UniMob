using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UniMob.UI
{
    public abstract class ScriptableStatefulWidget : ScriptableObject, Widget
    {
        private Type _type;

        [NotNull] public Type Type => _type ?? (_type = GetType());

        [CanBeNull] public abstract Key Key { get; }

        [NotNull]
        public abstract State CreateState();
    }
}