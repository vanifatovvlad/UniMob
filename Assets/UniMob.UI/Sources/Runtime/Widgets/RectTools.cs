using UnityEngine;

namespace UniMob.UI.Widgets
{
    internal static class RectTools
    {
        public static Vector2 PositionToAnchored(Vector2 position, Vector2 pivot, Vector2 size, Alignment corner)
        {
            return new Vector2(position.x, -position.y) +
                   new Vector2(size.x * pivot.x * -corner.X, -size.y * (1f - pivot.y) * -corner.Y);
        }
    }
}