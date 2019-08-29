using UnityEngine;
using UnityEngine.UI;

namespace UniMob.UI.Widgets
{
    [AddComponentMenu("UniMob/Views/LayoutViewPanel")]
    public sealed class LayoutViewPanel : ViewBase<IState[]>
    {
        [SerializeField] private bool driveSize = true;
        [SerializeField] private bool drivePosition = true;

        private ViewMapperBase _mapper;

        public void Render(IState[] state) => ((IView) this).SetSource(state);

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
                var children = State;

                foreach (var child in children)
                {
                    var childView = render.RenderItem(child);
                    var childSize = child.Size;

                    var alignment = Alignment.Center;

                    if (driveSize)
                    {
                        ViewLayoutUtility.SetSize(childView.rectTransform, childSize, alignment.ToAnchor());
                    }

                    if (drivePosition)
                    {
                        ViewLayoutUtility.SetPosition(childView.rectTransform, childSize, Vector2.zero, alignment);
                    }
                }
            }
        }
    }
}