namespace UniMob.UI
{
    public class FloatTween : IAnimatable<float>
    {
        public float From { get; }
        public float To { get; }

        public FloatTween(float from, float to)
        {
            From = from;
            To = to;
        }

        public float Transform(float t) => From + (To - From) * t;
    }
}