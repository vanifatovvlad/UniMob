using UnityEngine;

namespace UniMob.UI.Widgets
{
    public abstract class StatefulContainerWidget : StatefulWidget
    {
    }

    public abstract class StatefulContainerState<TContainerWidget> : State<TContainerWidget>, IContainerState
        where TContainerWidget : StatefulContainerWidget
    {
        private readonly Atom<IState> _child;

        public override WidgetViewReference View { get; }
            = WidgetViewReference.Resource("UniMob.Container");

        protected StatefulContainerState()
        {
            _child = CreateChild(Build);
        }

        protected abstract Widget Build(BuildContext context);

        public sealed override WidgetSize CalculateSize() => _child.Value.Size;

        public string DebugName => typeof(TContainerWidget).Name;
        
        Color IContainerState.BackgroundColor => Color.clear;

        Alignment IContainerState.Alignment => Alignment.Center;

        IState IContainerState.Child => _child.Value;
    }
}