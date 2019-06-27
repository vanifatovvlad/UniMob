namespace UniMob.ReView
{
    public interface IState
    {
        WidgetSize InnerSize { get; }
        WidgetSize OuterSize { get; }

        string ViewPath { get; }

        void DidViewMount();
        void DidViewUnmount();
    }
}