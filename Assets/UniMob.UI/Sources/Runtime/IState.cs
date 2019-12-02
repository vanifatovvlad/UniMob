namespace UniMob.UI
{
    public interface IState
    {
        WidgetSize Size { get; }

        BuildContext Context { get; }

        WidgetViewReference View { get; }

        void DidViewMount(IView view);
        void DidViewUnmount(IView view);
    }
}