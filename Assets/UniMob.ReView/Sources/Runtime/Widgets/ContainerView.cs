using UnityEngine;

namespace UniMob.ReView.Widgets
{
    [AddComponentMenu("UniMob/Views/Container")]
    public sealed class ContainerView : LayoutView<IState>
    {
        protected override void Render()
        {
            using (var render = Mapper.CreateRender())
            {
                var childView = render.RenderItem(State);
                var childSize = State.Size;

                var alignment = Alignment.Center;
                SetSize(childView.rectTransform, childSize, alignment.ToAnchor(), true, true);
                SetPosition(childView.rectTransform, childSize, Vector2.zero, alignment);
            }
        }
    }
}