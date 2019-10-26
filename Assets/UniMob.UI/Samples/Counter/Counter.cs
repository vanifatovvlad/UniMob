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

        public override Key Key { get; } = GlobalKey.Of<Counter>();

        public override State CreateState() => new CounterState();
    }

    public class CounterState : State<Counter>, ICounterState
    {
        private readonly MutableAtom<int> _counter = Atom.Value(0);

        public override WidgetViewReference View => Widget.ViewReference;

        public string Counter => $"Counter: {_counter.Value}";

        public void Increment() => _counter.Value = Math.Min(_counter.Value + 1, Widget.MaxValue);
        public void Decrement() => _counter.Value = Math.Max(_counter.Value - 1, Widget.MinValue);
    }
}