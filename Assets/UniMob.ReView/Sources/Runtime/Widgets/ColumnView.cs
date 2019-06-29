using UnityEngine;

namespace UniMob.ReView.Widgets
{
    [AddComponentMenu("UniMob/Views/Column")]
    public sealed class ColumnView : LayoutView<IColumnState>
    {
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

            using (var render = Mapper.CreateRender())
            {
                float y = -columnSize.Height * offsetMultiplierY;

                foreach (var child in children)
                {
                    var childSize = child.OuterSize;

                    if (childSize.IsHeightStretched)
                    {
                        Debug.LogError("Cannot render vertically stretched widgets inside Column.");
                        continue;
                    }

                    var childView = render.RenderItem(child);

                    var localAlignX = childSize.IsWidthStretched ? Alignment.Center.X : alignX;

                    var anchor = new Alignment(localAlignX, alignY);
                    SetSize(childView.rectTransform, childSize, anchor.ToAnchor());

                    var position = new Vector2(0, y);
                    var corner = new Alignment(localAlignX, Alignment.TopCenter.Y);
                    SetPosition(childView.rectTransform, childSize, position, corner);

                    y += childSize.Height;
                }
            }
        }
    }

    public interface IColumnState : IState
    {
        IState[] Children { get; }
        CrossAxisAlignment CrossAxisAlignment { get; }
        MainAxisAlignment MainAxisAlignment { get; }
    }
}