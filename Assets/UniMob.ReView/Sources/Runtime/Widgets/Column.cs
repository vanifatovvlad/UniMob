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
            key
        )
        {
            CrossAxisAlignment = crossAxisAlignment;
            MainAxisAlignment = mainAxisAlignment;
            StretchHorizontal = stretchHorizontal;
            StretchVertical = stretchVertical;
        }

        public CrossAxisAlignment CrossAxisAlignment { get; }
        public MainAxisAlignment MainAxisAlignment { get; }
        
        public bool StretchHorizontal { get; }
        public bool StretchVertical { get; }

        public override State CreateState() => new ColumnState();
    }
}