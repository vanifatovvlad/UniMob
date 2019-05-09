using System;
using UniMob.Async;

namespace UniMob.ReView.Samples.Counter
{
    public class Counter : Widget
    {
        public override State CreateState() => new CounterState();
    }

    public class CounterState : State<Counter>, ICounterState
    {
        private readonly MutableAtom<int> _counter = Atom.Value(0);

        public CounterState() : base("Counter")
        {
        }

        public string Counter => $"Counter: {_counter.Value}";

        public void Increment() => ++_counter.Value;
        public void Decrement() => _counter.Value = Math.Max(0, _counter.Value - 1);
    }
}