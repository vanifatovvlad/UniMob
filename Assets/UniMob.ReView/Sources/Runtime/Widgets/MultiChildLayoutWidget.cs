using System;
using JetBrains.Annotations;

namespace UniMob.ReView.Widgets
{
    public abstract class MultiChildLayoutWidget : LayoutWidget
    {
        protected MultiChildLayoutWidget(
            [NotNull] WidgetList children,
            [CanBeNull] Key key,
            bool stretchHorizontal,
            bool stretchVertical
        ) : base(
            key,
            stretchHorizontal,
            stretchVertical)
        {
            Children = children ?? throw new ArgumentNullException(nameof(children));
        }

        [NotNull] public WidgetList Children { get; }
    }
}