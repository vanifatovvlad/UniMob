using UnityEngine;

namespace UniMob.UI.Widgets
{
    internal static class ViewLayoutUtility
    {
        public static void SetPosition(RectTransform rt, WidgetSize size, Vector2 position, Alignment corner)
        {
            var sizeDeltaX = size.IsWidthFixed ? size.Width : 0;
            var sizeDeltaY = size.IsHeightFixed ? size.Height : 0;
            var sizeDelta = new Vector2(sizeDeltaX, sizeDeltaY);
            rt.anchoredPosition = RectTools.PositionToAnchored(position, rt.pivot, sizeDelta, corner);
        }

        public static void SetSize(RectTransform rt, WidgetSize size, Vector2 anchor)
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