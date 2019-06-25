namespace UniMob.ReView
{
    public interface IViewTreeElement
    {
        void AddChild(IViewTreeElement view);
        void Unmount();
    }
}