using System;
using JetBrains.Annotations;

namespace UniMob.UI.Widgets
{
    public abstract class SingleChildLayoutWidget : StatefulWidget
    {
        protected SingleChildLayoutWidget(
            [NotNull] Widget child,
            [CanBeNull] Key key
        ) : base(key)
        {
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }

        [NotNull] public Widget Child { get; }
    }

    internal abstract class SingleChildLayoutState<TWidget> : State<TWidget>
        where TWidget : SingleChildLayoutWidget
    {
        private readonly Atom<IState> _child;

        protected SingleChildLayoutState([NotNull] string view) : base(view)
        {
            _child = CreateChild(context => Widget.Child);
        }

        public IState Child => _child.Value;
    }
}