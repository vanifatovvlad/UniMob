using System;

namespace UniMob.UI.Samples.NumTree
{
    public class NumTreeButton : StatefulWidget
    {
        public NumTreeButton(Action onTap)
        {
            OnTap = onTap;
        }

        public Action OnTap { get; }

        public override State CreateState() => new NumTreeButtonState();
    }

    public class NumTreeButtonState : State<NumTreeButton>, INumTreeButtonState
    {
        public NumTreeButtonState() : base("NumTreeButton")
        {
        }

        public void Tap() => Widget.OnTap?.Invoke();
    }
}