namespace UniMob.ReView.Widgets
{
    public interface ILayoutState : IState
    {
        bool StretchVertical { get; }
        bool StretchHorizontal { get; }
    }
}