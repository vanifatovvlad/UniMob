using System;
using JetBrains.Annotations;

namespace UniMob.ReView.Widgets
{
    public abstract class SingleChildLayoutWidget : Widget
    {
        protected SingleChildLayoutWidget(
            [NotNull] Widget child,
            [CanBeNull] Key key
        ) : base(key)
        {
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }

        [NotNull] public Widget Child { get; }
    }
}