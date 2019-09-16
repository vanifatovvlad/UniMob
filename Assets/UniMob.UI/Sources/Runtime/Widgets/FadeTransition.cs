namespace UniMob.UI.Widgets
{
    using JetBrains.Annotations;

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
        public FadeTransitionState([NotNull] string view = "UniMob.FadeTransition") : base(view)
        {
        }

        public ITween<float> Opacity => Widget.Opacity;
    }
}