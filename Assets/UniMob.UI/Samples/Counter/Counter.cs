using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UniMob.UI.Samples.Counter
{
    public class Counter : StatefulWidget
    {
        public Counter(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Min { get; }

        public int Max { get; }

        public override State CreateState() => StateProvider.Of(this);
    }

    public class CounterState : ViewState<Counter>, ICounterState
    {
        [Atom] private int Counter { get; set; }

        public override WidgetViewReference View { get; }

        public CounterState(WidgetViewReference view)
        {
            View = view;
        }

        public override void InitState()
        {
            base.InitState();

            Counter = Widget.Min;
        }

        public override void DidUpdateWidget(Counter oldWidget)
        {
            base.DidUpdateWidget(oldWidget);

            Counter = Math.Max(Counter, Widget.Min);
            Counter = Math.Min(Counter, Widget.Max);
        }

        public string CounterText => $"Counter: {Counter}";

        public void Increment() => Counter = Math.Min(Counter + 1, Widget.Max);
        public void Decrement() => Counter = Math.Max(Counter - 1, Widget.Min);
    }
}