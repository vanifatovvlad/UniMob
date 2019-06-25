using System;
using UnityEngine;

namespace UniMob.ReView.Widgets
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
}