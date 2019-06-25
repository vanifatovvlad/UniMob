using UnityEngine;

namespace UniMob.ReView.Widgets
{
    [AddComponentMenu("UniMob/Views/Column")]
    public class ColumnView : LayoutView<ColumnState>
    {
        protected override void Render()
        {
            var children = State.Children;
            var crossAxis = State.CrossAxisAlignment;
            var mainAxis = State.MainAxisAlignment;
            var containerSize = State.Size;

            var alignX = crossAxis == CrossAxisAlignment.Start ? Alignment.TopLeft.X
                : crossAxis == CrossAxisAlignment.End ? Alignment.TopRight.X
                : Alignment.Center.X;

            var alignY = mainAxis == MainAxisAlignment.Start ? Alignment.TopCenter.Y
                : mainAxis == MainAxisAlignment.End ? Alignment.BottomCenter.Y
                : Alignment.Center.Y;

            var posMultY = mainAxis == MainAxisAlignment.Start ? 0
                : mainAxis == MainAxisAlignment.End ? 1f
                : 0.5f;

            using (var render = Mapper.CreateRender())
            {
                float y = -containerSize.y * posMultY;
                foreach (var child in children)
                {
                    var childSize = child.Size;
                    var childView = render.RenderItem(child);
                    var layoutState = child as ILayoutState;
                    var wStretch = layoutState?.StretchHorizontal ?? false;
                    var hStretch = layoutState?.StretchVertical ?? false;
                    
                    var anchor = new Alignment(wStretch ? Alignment.Center.X : alignX, alignY);
                    SetSize(childView.rectTransform, childSize, anchor.ToAnchor(), wStretch, hStretch);
                    
                    var position = new Vector2(0, y);
                    var corner = new Alignment(wStretch ? Alignment.Center.X : alignX, Alignment.TopCenter.Y);
                    SetPosition(childView.rectTransform, childSize, position, corner);

                    y += childSize.y;
                }
            }
        }
    }
}