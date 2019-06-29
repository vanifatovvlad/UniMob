using System;
using JetBrains.Annotations;
using UniMob.Async;

namespace UniMob.ReView.Widgets
{
    public abstract class MultiChildLayoutWidget : Widget
    {
        protected MultiChildLayoutWidget(
            [NotNull] WidgetList children,
            [CanBeNull] Key key
        ) : base(
            key)
        {
            Children = children ?? throw new ArgumentNullException(nameof(children));
        }

        [NotNull] public WidgetList Children { get; }
    }
    
    internal abstract class MultiChildLayoutState<TWidget> : State<TWidget>
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