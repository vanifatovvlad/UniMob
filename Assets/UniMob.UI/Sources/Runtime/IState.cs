namespace UniMob.UI
{
    public interface IState
    {
        WidgetSize Size { get; }

        string ViewPath { get; }

        void DidViewMount();
        void DidViewUnmount();
    }
}