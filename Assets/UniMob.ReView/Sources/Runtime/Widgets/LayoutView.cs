using UnityEngine;

namespace UniMob.ReView.Widgets
{
    public abstract class LayoutView<TState> : View<TState>
        where TState : IState
    {
        protected ViewMapperBase Mapper { get; private set; }

        protected override void Activate()
        {
            base.Activate();

            if (Mapper == null)
                Mapper = new PooledViewMapper(transform);
        }

        protected static void SetPosition(RectTransform rt, WidgetSize size, Vector2 position, Alignment corner)
        {
            var sizeDeltaX = size.IsWidthFixed ? size.Width : 0;
            var sizeDeltaY = size.IsHeightFixed ? size.Height : 0;
            var sizeDelta = new Vector2(sizeDeltaX, sizeDeltaY);
            rt.anchoredPosition = RectTools.PositionToAnchored(position, rt.pivot, sizeDelta, corner);
        }

        protected static void SetSize(RectTransform rt, WidgetSize size, Vector2 anchor)
        {
            var sizeDeltaX = size.IsWidthFixed ? size.Width : 0;
            var sizeDeltaY = size.IsHeightFixed ? size.Height : 0;
            var sizeDelta = new Vector2(sizeDeltaX, sizeDeltaY);
            
            var anchorMin = anchor;
            var anchorMax = anchor;

            if (size.IsWidthStretched)
            {
                anchorMin.x = 0;
                anchorMax.x = 1;
            }

            if (size.IsHeightStretched)
            {
                anchorMin.y = 0;
                anchorMax.y = 1;
            }

            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.sizeDelta = sizeDelta;
        }
    }
}