namespace UniMob.UI.Widgets
{
    using Internal;
    using UnityEngine;

    public class NavigatorView : View<INavigatorState>
    {
        private ViewMapperBase _mapper;

        protected override void Activate()
        {
            base.Activate();

            if (_mapper == null)
                _mapper = new PooledViewMapper(transform);
        }

        protected override void Render()
        {
            using (var render = _mapper.CreateRender()) {
                var children = State.Screens;
                foreach (var child in children) {
                    var childView = render.RenderItem(child);
                    var childSize = child.Size;

                    var alignment = Alignment.Center;
                    ViewLayoutUtility.SetSize(childView.rectTransform, childSize, alignment.ToAnchor());
                    ViewLayoutUtility.SetPosition(childView.rectTransform, childSize, Vector2.zero, alignment);
                }
            }
        }
    }

    public interface INavigatorState : IViewState
    {
        IState[] Screens { get; }
    }
}