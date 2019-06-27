using System;
using JetBrains.Annotations;

namespace UniMob.ReView.Widgets
{
    public abstract class MultiChildLayoutWidget : Widget
    {
        protected MultiChildLayoutWidget(
            [NotNull] WidgetList children,
            [CanBeNull] Key key
        ) : base(
            key)
        {
            Children = children ?? throw new ArgumentNullException(nameof(children));
        }

        [NotNull] public WidgetList Children { get; }
    }
}