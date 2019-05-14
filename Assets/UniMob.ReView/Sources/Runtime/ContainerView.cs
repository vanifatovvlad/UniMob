using UnityEngine;

namespace UniMob.ReView
{
    [AddComponentMenu("UniMob/Views/Container")]
    public sealed class ContainerView : LayoutView<IState>
    {
        public void SetState(IState state) => ((IView) this).SetSource(state);

        protected override void Render()
        {
            try
            {
                Mapper.BeginRender();
                var childView = Mapper.RenderItem(State);
                var childSize = State.Size;

                var alignment = Alignment.Center;
                SetSize(childView.rectTransform, childSize, alignment.ToAnchor(), true, true);
                SetPosition(childView.rectTransform, childSize, Vector2.zero, alignment);
            }
            finally
            {
                Mapper.EndRender();
            }
        }
    }
}