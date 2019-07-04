using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniMob.Async;

namespace UniMob.ReView.Widgets
{
    public abstract class MultiChildLayoutWidget : Widget
    {
        protected MultiChildLayoutWidget(
            [NotNull] List<Widget> children,
            [CanBeNull] Key key
        ) : base(
            key)
        {
            Children = children ?? throw new ArgumentNullException(nameof(children));
        }

        [NotNull] public List<Widget> Children { get; }
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