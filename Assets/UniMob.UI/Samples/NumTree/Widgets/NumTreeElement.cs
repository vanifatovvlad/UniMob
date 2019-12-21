using UniMob.UI.Samples.NumTree.Views;

namespace UniMob.UI.Samples.NumTree.Widgets
{
    public class NumTreeElement : StatefulWidget
    {
        public NumTreeElement(Atom<int> valueAtom)
        {
            ValueAtom = valueAtom;
        }

        public Atom<int> ValueAtom { get; }

        public override State CreateState() => new NumTreeElementState();
    }

    public class NumTreeElementState : ViewState<NumTreeElement>, INumTreeElementViewState
    {
        public override WidgetViewReference View { get; }
            = WidgetViewReference.Addressable("UniMob.UI/Samples/NumTree/NumTreeElement.prefab");

        public int Value => Widget.ValueAtom.Value;
    }
}