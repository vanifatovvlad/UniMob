using UniMob.UI.Internal;
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

            name = State.DebugName;
        }

        protected override void Render()
        {
            var backgroundColor = State.BackgroundColor;
            var transparent = backgroundColor == Color.clear;

            backgroundImage.enabled = !transparent;
            backgroundImage.color = backgroundColor;

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

    public interface IContainerState : IViewState
    {
        string DebugName { get; }

        Color BackgroundColor { get; }

        Alignment Alignment { get; }

        IState Child { get; }
    }
}