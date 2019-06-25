using UnityEngine;

namespace UniMob.ReView.Widgets
{
    public class ColumnState : MultiChildLayoutState<Column>
    {
        public ColumnState() : base("UniMob.Column")
        {
        }

        public CrossAxisAlignment CrossAxisAlignment => Widget.CrossAxisAlignment;
        public MainAxisAlignment MainAxisAlignment => Widget.MainAxisAlignment;

        public override Vector2 CalculateSize()
        {
            float height = 0;
            float width = 0;

            foreach (var child in Children)
            {
                var childSize = child.Size;
                height += childSize.y;
                width = Mathf.Max(width, childSize.x);
            }

            return new Vector2(width, height);
        }
    }
}