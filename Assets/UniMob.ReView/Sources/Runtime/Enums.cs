using System;
using UnityEngine;

namespace UniMob.ReView
{
    [Serializable]
    public class RectPadding
    {
        [SerializeField] private float left;
        [SerializeField] private float right;
        [SerializeField] private float top;
        [SerializeField] private float bottom;

        public float Left => left;
        public float Right => right;
        public float Top => top;
        public float Bottom => bottom;

        public float Horizontal => Left + Right;
        public float Vertical => Top + Bottom;
    }

    public struct Alignment
    {
        public float X { get; }
        public float Y { get; }

        public Alignment(float x, float y)
        {
            X = x;
            Y = y;
        }

        internal Vector2 ToAnchor() => new Vector2(X * 0.5f + 0.5f, -Y * 0.5f + 0.5f);

        /// <summary>
        /// The center point along the bottom edge.
        /// </summary>
        public static readonly Alignment BottomCenter = new Alignment(0.0f, 1.0f);

        /// <summary>
        ///  The bottom left corner.
        /// </summary>
        public static readonly Alignment BottomLeft = new Alignment(-1.0f, 1.0f);

        /// <summary>
        /// The bottom right corner.
        /// </summary>
        public static readonly Alignment BottomRight = new Alignment(1.0f, 1.0f);

        /// <summary>
        /// The center point, both horizontally and vertically.
        /// </summary>
        public static readonly Alignment Center = new Alignment(0.0f, 0.0f);

        /// <summary>
        /// The center point along the left edge.
        /// </summary>
        public static readonly Alignment CenterLeft = new Alignment(-1.0f, 0.0f);

        /// <summary>
        /// The center point along the right edge.
        /// </summary>
        public static readonly Alignment CenterRight = new Alignment(1.0f, 0.0f);

        /// <summary>
        /// The center point along the top edge.
        /// </summary>
        public static readonly Alignment TopCenter = new Alignment(0.0f, -1.0f);

        /// <summary>
        /// The top left corner.
        /// </summary>
        public static readonly Alignment TopLeft = new Alignment(-1.0f, -1.0f);

        /// <summary>
        /// The top right corner.
        /// </summary>
        public static readonly Alignment TopRight = new Alignment(1.0f, -1.0f);
    }

    public enum CrossAxisAlignment
    {
        /// <summary>
        /// Place the children with their start edge aligned with the start side of the cross axis.
        /// </summary>
        Start = 0,

        /// <summary>
        /// Place the children as close to the end of the cross axis as possible.
        /// </summary>
        End = 1,

        /// <summary>
        /// Place the children so that their centers align with the middle of the cross axis.
        /// </summary>
        Center = 2,
        /*
        /// <summary>
        /// This causes the constraints passed to the children to be tight in the cross axis.
        /// </summary>
        Stretch = 3,
        */
    }

    public enum MainAxisAlignment
    {
        /// <summary>
        /// Place the children as close to the start of the main axis as possible.
        /// </summary>
        Start = 0,

        /// <summary>
        /// Place the children as close to the end of the main axis as possible.
        /// </summary>
        End = 1,

        /// <summary>
        /// Place the children as close to the middle of the main axis as possible.
        /// </summary>
        Center = 2,

        /// <summary>
        /// Place the free space evenly between the children as well as half
        /// of that space before and after the first and last child.
        /// </summary>
        SpaceAround = 4,

        /// <summary>
        /// Place the free space evenly between the children as well as before and after the first and last child.
        /// </summary>
        SpaceEvenly = 5,
    }

    public enum MainAxisSize
    {
        /// <summary>
        /// Minimize the amount of free space along the main axis, subject to the incoming layout constraints.
        /// </summary>
        Min = 0,

        /// <summary>
        /// Maximize the amount of free space along the main axis, subject to the incoming layout constraints.
        /// </summary>
        Max = 1,
    }
}