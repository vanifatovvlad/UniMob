using JetBrains.Annotations;
using UniMob.Async;

namespace UniMob.ReView.Widgets
{
    public abstract class SingleChildLayoutState<TWidget> : State<TWidget>
        where TWidget : SingleChildLayoutWidget
    {
        private readonly Atom<IState> _child;

        protected SingleChildLayoutState([NotNull] string view) : base(view)
        {
            _child = Create(this, context => Widget.Child);
        }

        public IState Child => _child.Value;
    }
}