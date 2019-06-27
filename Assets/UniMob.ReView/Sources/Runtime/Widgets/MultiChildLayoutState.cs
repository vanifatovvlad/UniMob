using UniMob.Async;

namespace UniMob.ReView.Widgets
{
    public abstract class MultiChildLayoutState<TWidget> : State<TWidget>
        where TWidget : MultiChildLayoutWidget
    {
        private readonly Atom<IState[]> _children;

        protected MultiChildLayoutState(string view) : base(view)
        {
            _children = CreateList(this, context => Widget.Children);
        }

        public IState[] Children => _children.Value;
    }
}