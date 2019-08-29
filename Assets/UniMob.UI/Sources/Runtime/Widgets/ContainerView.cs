using UnityEngine;
using UnityEngine.UI;

namespace UniMob.UI.Widgets
{
    [AddComponentMenu("UniMob/Views/Container")]
    public sealed class ContainerView : View<IContainerState>
    {
        [SerializeField] private Image backgroundImage = default;

        private ViewMapperBase _mapper;

        protected override void Activate()
        {
            base.Activate();

            if (_mapper == null)
                _mapper = new PooledViewMapper(transform);
        }

        protected override void Render()
        {
            var color = State.Color;
            var transparent = color == Color.clear;

            backgroundImage.enabled = !transparent;
            backgroundImage.color = color;

            using (var render = _mapper.CreateRender())
            {
                var child = State.Child;
                var childView = render.RenderItem(child);
                var childSize = child.Size;

                var alignment = State.Alignment;
                ViewLayoutUtility.SetSize(childView.rectTransform, childSize, alignment.ToAnchor());
                ViewLayoutUtility.SetPosition(childView.rectTransform, childSize, Vector2.zero, alignment);
            }
        }
    }

    public interface IContainerState : IState
    {
        Color Color { get; }

        Alignment Alignment { get; }

        IState Child { get; }
    }
}