using JetBrains.Annotations;

namespace UniMob.ReView.Widgets
{
    public class Column : MultiChildLayoutWidget
    {
        public Column(
            [NotNull] WidgetList children,
            [CanBeNull] Key key = null,
            CrossAxisAlignment crossAxisAlignment = CrossAxisAlignment.Start,
            MainAxisAlignment mainAxisAlignment = MainAxisAlignment.Start,
            bool stretchHorizontal = false,
            bool stretchVertical = false
        ) : base(
            children,
            key,
            stretchHorizontal,
            stretchVertical
        )
        {
            CrossAxisAlignment = crossAxisAlignment;
            MainAxisAlignment = mainAxisAlignment;
        }

        public CrossAxisAlignment CrossAxisAlignment { get; }
        public MainAxisAlignment MainAxisAlignment { get; }

        public override State CreateState() => new ColumnState();
    }
}