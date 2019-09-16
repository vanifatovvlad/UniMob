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
        public override WidgetViewReference View { get; }
            = WidgetViewReference.Addressable("UniMob.UI/Samples/NumTree/NumTreeButton.prefab");

        public void Tap() => Widget.OnTap?.Invoke();
    }
}