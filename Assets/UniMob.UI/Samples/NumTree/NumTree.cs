using System.Collections.Generic;
using System.Linq;
using UniMob.UI.Widgets;
using UnityEngine;

namespace UniMob.UI.Samples.NumTree
{
    public class NumTree : StatefulContainerWidget
    {
        public NumTree(NumTreeModel model)
        {
            Model = model;
        }

        public NumTreeModel Model { get; }

        public override State CreateState() => new NumTreeState();
    }

    public class NumTreeState : StatefulContainerState<NumTree>
    {
        private NumTreeModel Model => Widget.Model;
        
        protected override Widget Build(BuildContext context)
        {
            return new Container(
                backgroundColor: Color.white,
                child: new Column(
                    mainAxisSize: AxisSize.Max,
                    crossAxisSize: AxisSize.Max,
                    mainAxisAlignment: MainAxisAlignment.Center,
                    crossAxisAlignment: CrossAxisAlignment.Center,
                    children: Enumerable.Empty<Widget>()
                        .Concat(BuildTreeRows(Model.Tree))
                        .Append(BuildButtonsRow(Model.Roots))
                        .ToList()
                )
            );
        }
        
        private IEnumerable<Widget> BuildTreeRows(Atom<int>[][] grid)
        {
            return grid.Select(line =>
            {
                return (Widget) new Row(
                    mainAxisSize: AxisSize.Max,
                    mainAxisAlignment: MainAxisAlignment.Center,
                    children: line.Select(item => (Widget) new NumTreeElement(item)).ToList()
                );
            });
        }

        private Widget BuildButtonsRow(MutableAtom<int>[] roots)
        {
            var buttonIndexes = Enumerable.Range(0, roots.Length);

            return new Row(
                mainAxisSize: AxisSize.Max,
                mainAxisAlignment: MainAxisAlignment.Center,
                children: buttonIndexes.Select(index =>
                    {
                        return (Widget) new NumTreeButton(
                            onTap: () => ++roots[index].Value);
                    })
                    .ToList()
            );
        }
    }
}