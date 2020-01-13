namespace UniMob.UI.Widgets
{
    public class FadeTransition : SingleChildLayoutWidget
    {
        public FadeTransition(
            Widget child,
            IAnimation<float> opacity,
            Key key = null
        ) : base(child, key)
        {
            Opacity = opacity;
        }

        public IAnimation<float> Opacity { get; }

        public override State CreateState() => new FadeTransitionState();
    }

    internal class FadeTransitionState : SingleChildLayoutState<FadeTransition>, IFadeTransitionState
    {
        public override WidgetViewReference View { get; }
            = WidgetViewReference.Resource("$$_FadeTransition");

        public IAnimation<float> Opacity => Widget.Opacity;
        public Alignment Alignment => Alignment.Center;
    }
}