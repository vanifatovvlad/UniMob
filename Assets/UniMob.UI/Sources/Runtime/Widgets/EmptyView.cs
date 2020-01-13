namespace UniMob.UI.Widgets
{
    public class EmptyView : View<IEmptyState>
    {
        protected override void Render()
        {
        }
    }

    public interface IEmptyState : IViewState
    {
    }
}