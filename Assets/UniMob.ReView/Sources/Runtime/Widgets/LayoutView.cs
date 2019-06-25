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

        protected static void SetPosition(RectTransform rt, Vector2 size, Vector2 position, Alignment corner)
        {
            rt.anchoredPosition = RectTools.PositionToAnchored(position, rt.pivot, size, corner);
        }

        protected static void SetSize(RectTransform rt, Vector2 size, Vector2 anchor,
            bool widthStretch, bool heightStretch)
        {
            var sizeDelta = size;
            var anchorMin = anchor;
            var anchorMax = anchor;

            if (widthStretch)
            {
                sizeDelta.x = 0;
                anchorMin.x = 0;
                anchorMax.x = 1;
            }

            if (heightStretch)
            {
                sizeDelta.y = 0;
                anchorMin.y = 0;
                anchorMax.y = 1;
            }

            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.sizeDelta = sizeDelta;
        }
    }
}