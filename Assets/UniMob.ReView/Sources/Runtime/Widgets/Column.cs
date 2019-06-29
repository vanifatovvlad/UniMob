using JetBrains.Annotations;
using UnityEngine;

namespace UniMob.ReView.Widgets
{
    public sealed class Column : MultiChildLayoutWidget
    {
        public Column(
            [NotNull] WidgetList children,
            [CanBeNull] Key key = null,
            CrossAxisAlignment crossAxisAlignment = CrossAxisAlignment.Start,
            MainAxisAlignment mainAxisAlignment = MainAxisAlignment.Start,
            AxisSize crossAxisSize = AxisSize.Min,
            AxisSize mainAxisSize = AxisSize.Min
        ) : base(
            children,
            key
        )
        {
            CrossAxisAlignment = crossAxisAlignment;
            MainAxisAlignment = mainAxisAlignment;
            CrossAxisSize = crossAxisSize;
            MainAxisSize = mainAxisSize;
        }

        public CrossAxisAlignment CrossAxisAlignment { get; }
        public MainAxisAlignment MainAxisAlignment { get; }

        public AxisSize CrossAxisSize { get; }
        public AxisSize MainAxisSize { get; }

        public override State CreateState() => new ColumnState();
    }

    internal sealed class ColumnState : MultiChildLayoutState<Column>, IColumnState
    {
        public ColumnState() : base("UniMob.Column")
        {
        }

        public CrossAxisAlignment CrossAxisAlignment => Widget.CrossAxisAlignment;
        public MainAxisAlignment MainAxisAlignment => Widget.MainAxisAlignment;

        public override WidgetSize CalculateOuterSize()
        {
            var wStretch = Widget.CrossAxisSize == AxisSize.Max;
            var hStretch = Widget.MainAxisSize == AxisSize.Max;

            if (wStretch && hStretch)
            {
                return WidgetSize.Stretched;
            }

            var size = base.CalculateOuterSize();

            float? width = null;
            float? height = null;

            if (size.IsWidthFixed && !wStretch) width = size.Width;
            if (size.IsHeightFixed && !hStretch) height = size.Height;

            return new WidgetSize(width, height);
        }

        public override WidgetSize CalculateInnerSize()
        {
            float height = 0;
            float? width = 0;

            foreach (var child in Children)
            {
                var childSize = child.OuterSize;

                if (childSize.IsHeightFixed)
                {
                    height += childSize.Height;
                }

                if (width.HasValue && childSize.IsWidthFixed)
                {
                    width = Mathf.Max(width.Value, childSize.Width);
                }
                else
                {
                    width = null;
                }
            }

            return new WidgetSize(width, height);
        }
    }
}