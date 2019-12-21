using System.Collections.Generic;
using System.Linq;
using UniMob.UI.Widgets;
using UnityEngine;

namespace UniMob.UI.Samples.NumTree.Widgets
{
    public class NumTree : StatefulWidget
    {
        public override State CreateState() => new NumTreeState();
    }

    public class NumTreeState : HocState<NumTree>
    {
        private readonly NumTreeModel _model = new NumTreeModel(levels: 11);

        public override Widget Build(BuildContext context)
        {
            return new Container(
                backgroundColor: Color.white,
                child: new Column(
                    mainAxisSize: AxisSize.Max,
                    crossAxisSize: AxisSize.Max,
                    mainAxisAlignment: MainAxisAlignment.Center,
                    crossAxisAlignment: CrossAxisAlignment.Center,
                    children: BuildTree()
                )
            );
        }

        private List<Widget> BuildTree()
        {
            return BuildRows(_model.Tree)
                .Append(BuildButtons(_model.Roots))
                .ToList();
        }

        private List<Widget> BuildRows(Atom<int>[][] grid)
        {
            return grid
                .Select(BuildRow)
                .ToList();
        }

        private Widget BuildRow(Atom<int>[] line)
        {
            return new Row(
                mainAxisSize: AxisSize.Max,
                mainAxisAlignment: MainAxisAlignment.Center,
                children: line.Select(BuildElement).ToList()
            );
        }

        private Widget BuildElement(Atom<int> element)
        {
            return new NumTreeElement(element);
        }

        private Widget BuildButtons(MutableAtom<int>[] roots)
        {
            return new Row(
                mainAxisSize: AxisSize.Max,
                mainAxisAlignment: MainAxisAlignment.Center,
                children: roots.Select(BuildButton).ToList()
            );
        }

        private Widget BuildButton(MutableAtom<int> root)
        {
            return new NumTreeButton(
                onTap: () => ++root.Value
            );
        }
    }
}