using UniMob.UI.Internal;
using UnityEngine;

namespace UniMob.UI.Widgets
{
    [AddComponentMenu("UniMob/Views/Column")]
    public sealed class ColumnView : View<IColumnState>
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
            var children = State.Children;
            var crossAxis = State.CrossAxisAlignment;
            var mainAxis = State.MainAxisAlignment;
            var columnSize = State.InnerSize;

            var alignX = crossAxis == CrossAxisAlignment.Start ? Alignment.TopLeft.X
                : crossAxis == CrossAxisAlignment.End ? Alignment.TopRight.X
                : Alignment.Center.X;

            var alignY = mainAxis == MainAxisAlignment.Start ? Alignment.TopCenter.Y
                : mainAxis == MainAxisAlignment.End ? Alignment.BottomCenter.Y
                : Alignment.Center.Y;

            var offsetMultiplierY = mainAxis == MainAxisAlignment.Start ? 0
                : mainAxis == MainAxisAlignment.End ? 1f
                : 0.5f;

            using (var render = _mapper.CreateRender())
            {
                float y = -columnSize.Height * offsetMultiplierY;

                foreach (var child in children)
                {
                    var childSize = child.Size;

                    if (childSize.IsHeightStretched)
                    {
                        Debug.LogError("Cannot render vertically stretched widgets inside Column.");
                        continue;
                    }

                    var childView = render.RenderItem(child);

                    var localAlignX = childSize.IsWidthStretched ? Alignment.Center.X : alignX;

                    var anchor = new Alignment(localAlignX, alignY);
                    ViewLayoutUtility.SetSize(childView.rectTransform, childSize, anchor.ToAnchor());

                    var position = new Vector2(0, y);
                    var corner = new Alignment(localAlignX, Alignment.TopCenter.Y);
                    ViewLayoutUtility.SetPosition(childView.rectTransform, childSize, position, corner);

                    y += childSize.Height;
                }
            }
        }
    }

    public interface IColumnState : IViewState
    {
        WidgetSize InnerSize { get; }
        IState[] Children { get; }
        CrossAxisAlignment CrossAxisAlignment { get; }
        MainAxisAlignment MainAxisAlignment { get; }
    }
}