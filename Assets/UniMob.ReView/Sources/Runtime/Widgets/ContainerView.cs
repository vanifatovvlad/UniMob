using UnityEngine;
using UnityEngine.UI;

namespace UniMob.ReView.Widgets
{
    [AddComponentMenu("UniMob/Views/Container")]
    public sealed class ContainerView : LayoutView<IContainerState>
    {
        [SerializeField] private Image backgroundImage;

        protected override void Render()
        {
            var color = State.Color;
            var transparent = color == Color.clear;

            backgroundImage.enabled = !transparent;
            backgroundImage.color = color;

            using (var render = Mapper.CreateRender())
            {
                var child = State.Child;
                var childView = render.RenderItem(child);
                var childSize = child.OuterSize;

                var alignment = State.Alignment;
                SetSize(childView.rectTransform, childSize, alignment.ToAnchor());
                SetPosition(childView.rectTransform, childSize, Vector2.zero, alignment);
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