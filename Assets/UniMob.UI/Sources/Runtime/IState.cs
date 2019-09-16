namespace UniMob.UI
{
    public interface IState
    {
        WidgetSize Size { get; }

        WidgetViewReference View { get; }

        void DidViewMount();
        void DidViewUnmount();
    }
}