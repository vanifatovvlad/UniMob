using UniMob.UI.Internal;
using UnityEngine;

namespace UniMob.UI.Widgets
{
    [AddComponentMenu("UniMob/Views/Row")]
    public sealed class RowView : View<IRowState>
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

            var alignX = mainAxis == MainAxisAlignment.Start ? Alignment.TopLeft.X
                : mainAxis == MainAxisAlignment.End ? Alignment.TopRight.X
                : Alignment.Center.X;

            var alignY = crossAxis == CrossAxisAlignment.Start ? Alignment.TopCenter.Y
                : crossAxis == CrossAxisAlignment.End ? Alignment.BottomCenter.Y
                : Alignment.Center.Y;

            var offsetMultiplierX = mainAxis == MainAxisAlignment.Start ? 0
                : mainAxis == MainAxisAlignment.End ? 1f
                : 0.5f;

            using (var render = _mapper.CreateRender())
            {
                float x = -columnSize.Width * offsetMultiplierX;

                foreach (var child in children)
                {
                    var childSize = child.Size;

                    if (childSize.IsWidthStretched)
                    {
                        Debug.LogError("Cannot render horizontally stretched widgets inside Row.");
                        continue;
                    }

                    var childView = render.RenderItem(child);

                    var localAlignY = childSize.IsHeightStretched ? Alignment.Center.Y : alignY;

                    var anchor = new Alignment(alignX, localAlignY);
                    ViewLayoutUtility.SetSize(childView.rectTransform, childSize, anchor.ToAnchor());

                    var position = new Vector2(x, 0);
                    var corner = new Alignment(Alignment.CenterLeft.X, localAlignY);
                    ViewLayoutUtility.SetPosition(childView.rectTransform, childSize, position, corner);

                    x += childSize.Width;
                }
            }
        }
    }

    public interface IRowState : IState
    {
        WidgetSize InnerSize { get; }
        IState[] Children { get; }
        CrossAxisAlignment CrossAxisAlignment { get; }
        MainAxisAlignment MainAxisAlignment { get; }
    }
}