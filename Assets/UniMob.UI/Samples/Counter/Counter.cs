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

    public class CounterState : State<Counter>, ICounterState
    {
        [Atom] private int Counter { get; set; } = 0;

        public override WidgetViewReference View { get; }

        public CounterState(WidgetViewReference view)
        {
            View = view;
        }

        public string CounterText => $"Counter: {Counter}";

        public void Increment() => Counter = Math.Min(Counter + 1, Widget.Max);
        public void Decrement() => Counter = Math.Max(Counter - 1, Widget.Min);
    }
}