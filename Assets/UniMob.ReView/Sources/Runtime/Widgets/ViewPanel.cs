using UnityEngine;

namespace UniMob.ReView.Widgets
{
    [AddComponentMenu("UniMob/Views/ViewPanel")]
    public sealed class ViewPanel : LayoutView<IState>
    {
        protected override void Render()
        {
            using (var render = Mapper.CreateRender())
            {
                var childView = render.RenderItem(State);
                var childSize = State.Size;

                var alignment = Alignment.Center;
                SetSize(childView.rectTransform, childSize, alignment.ToAnchor());
                SetPosition(childView.rectTransform, childSize, Vector2.zero, alignment);
            }
        }
    }
}