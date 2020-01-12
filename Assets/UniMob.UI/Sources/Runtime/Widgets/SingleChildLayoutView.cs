using UniMob.UI.Internal;
using UnityEngine;

namespace UniMob.UI.Widgets
{
    public class SingleChildLayoutView<TState> : View<TState>
        where TState : ISingleChildLayoutState
    {
        private ViewMapperBase _mapper;

        protected override void Activate()
        {
            if (_mapper == null)
                _mapper = new PooledViewMapper(transform);

            base.Activate();
        }

        protected override void Render()
        {
            var alignment = State.Alignment;
            
            using (var render = _mapper.CreateRender())
            {
                var child = State.Child;
                var childView = render.RenderItem(child);
                var childSize = child.Size;

                LayoutData layout;
                layout.Size = childSize;
                layout.Alignment = alignment;
                layout.Corner = alignment;
                layout.CornerPosition = Vector2.zero;
                ViewLayoutUtility.SetLayout(childView.rectTransform, layout);
            }
        }
    }

    public interface ISingleChildLayoutState : IViewState
    {
        Alignment Alignment { get; }
        IState Child { get; }
    }
}