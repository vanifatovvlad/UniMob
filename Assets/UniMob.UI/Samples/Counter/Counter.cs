using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UniMob.UI.Samples.Counter
{
    [CreateAssetMenu(menuName = "UniMob UI Samples/Counter", fileName = "Counter Widget")]
    public class Counter : ScriptableStatefulWidget
    {
        [SerializeField] private AssetReferenceGameObject view;
        [SerializeField] private int minValue = 0;
        [SerializeField] private int maxValue = 10;

        public WidgetViewReference ViewReference => view;

        public int MinValue => minValue;

        public int MaxValue => maxValue;

        public override Key Key { get; } = GlobalKey.Of<CounterState>();

        public override State CreateState() => new CounterState();
    }

    public class CounterState : State<Counter>, ICounterState
    {
        [Atom] private int Counter { get; set; } = 0;

        public override WidgetViewReference View => Widget.ViewReference;

        public string CounterText => $"Counter: {Counter}";

        public void Increment() => Counter = Math.Min(Counter + 1, Widget.MaxValue);
        public void Decrement() => Counter = Math.Max(Counter - 1, Widget.MinValue);
    }
}