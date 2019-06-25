using JetBrains.Annotations;

namespace UniMob.ReView.Widgets
{
    public abstract class LayoutWidget : Widget
    {
        protected LayoutWidget(
            [CanBeNull] Key key,
            bool stretchHorizontal,
            bool stretchVertical
        ) : base(key)
        {
            StretchHorizontal = stretchHorizontal;
            StretchVertical = stretchVertical;
        }

        public bool StretchHorizontal { get; }
        public bool StretchVertical { get; }
    }
}