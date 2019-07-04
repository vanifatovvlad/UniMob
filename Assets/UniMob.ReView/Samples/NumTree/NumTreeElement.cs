using JetBrains.Annotations;
using UniMob.Async;

namespace UniMob.ReView.Samples.NumTree
{
    public class NumTreeElement : Widget
    {
        public NumTreeElement(Atom<int> valueAtom)
        {
            ValueAtom = valueAtom;
        }

        public Atom<int> ValueAtom { get; }

        public override State CreateState() => new NumTreeElementState();
    }

    public class NumTreeElementState : State<NumTreeElement>, INumTreeElementViewState
    {
        public NumTreeElementState() : base("NumTreeElement")
        {
        }

        public int Value => Widget.ValueAtom.Value;
    }
}