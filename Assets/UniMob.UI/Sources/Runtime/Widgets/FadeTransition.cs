namespace UniMob.UI.Widgets
{
    public class FadeTransition : SingleChildLayoutWidget
    {
        public FadeTransition(
            Widget child,
            ITween<float> opacity,
            Key key = null
        ) : base(child, key)
        {
            Opacity = opacity;
        }

        public ITween<float> Opacity { get; }

        public override State CreateState() => new FadeTransitionState();
    }

    internal class FadeTransitionState : SingleChildLayoutState<FadeTransition>, IFadeTransitionState
    {
        public override WidgetViewReference View { get; }
            = WidgetViewReference.Resource("$$_FadeTransition");

        public ITween<float> Opacity => Widget.Opacity;
    }
}