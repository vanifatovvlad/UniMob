using UniMob.UI.Internal;
using UnityEngine;

namespace UniMob.UI.Widgets
{
    [AddComponentMenu("UniMob/Views/ViewPanel")]
    public sealed class ViewPanel : ViewBase<IState>
    {
        private ViewMapperBase _mapper;

        public void Render(IState state) => ((IView) this).SetSource(state);

        protected override void Activate()
        {
            base.Activate();

            if (_mapper == null)
                _mapper = new PooledViewMapper(transform);
        }

        protected override void Render()
        {
            using (var render = _mapper.CreateRender())
            {
                var child = State;
                
                var childView = render.RenderItem(child);
                var childSize = child.Size;

                var alignment = Alignment.Center;
                ViewLayoutUtility.SetSize(childView.rectTransform, childSize, alignment.ToAnchor());
                ViewLayoutUtility.SetPosition(childView.rectTransform, childSize, Vector2.zero, alignment);
            }
        }
    }
}