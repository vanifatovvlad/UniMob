using System;
using UnityEngine;

namespace UniMob.ReView
{
    internal static class RectTools
    {
        /*
        public static Vector2 TopLeftCornerPositionToAnchored(Vector2 position, Vector2 pivot, Vector2 size)
        {
            return position + new Vector2(size.x * pivot.x, -size.y * (1f - pivot.y));
        }
        */

        public static bool IsStretchAnchors(float min, float max)
        {
            return Math.Abs(min) < Mathf.Epsilon && Math.Abs(max - 1) < Mathf.Epsilon;
        }

        public static void SetCenterAnchors(out float min, out float max)
        {
            min = max = 0.5f;
        }

        public static void SetStretchAnchors(out float min, out float max)
        {
            min = 0.0f;
            max = 1.0f;
        }

        public static class Corner
        {
            public static readonly Vector2 TopLeft = new Vector2(1, 1);
            public static readonly Vector2 MiddleLeft = new Vector2(1, 0);
            public static readonly Vector2 TopCenter = new Vector2(0, 1);
        }

        public static Vector2 CornerPositionToAnchored(Vector2 position, Vector2 pivot, Vector2 size, Vector2 corner)
        {
            return position + new Vector2(size.x * pivot.x * corner.x, -size.y * (1f - pivot.y) * corner.y);
        }
    }
}