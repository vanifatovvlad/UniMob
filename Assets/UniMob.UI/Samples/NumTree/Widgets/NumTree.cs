using System.Collections.Generic;
using System.Linq;
using UniMob.UI.Widgets;
using UnityEngine;

namespace UniMob.UI.Samples.NumTree.Widgets
{
    public class NumTree : StatefulContainerWidget
    {
        public override State CreateState() => new NumTreeState();
    }

    public class NumTreeState : StatefulContainerState<NumTree>
    {
        private readonly NumTreeModel _model = new NumTreeModel(levels: 11);

        private Timer _updateTimer;

        public override void InitState()
        {
            base.InitState();

            _updateTimer = Timer.RunPeriodic(0.1f, () =>
            {
                var rootIndex = Random.Range(0, _model.Roots.Length);
                var rootAtom = _model.Roots[rootIndex];
                rootAtom.Value = (rootAtom.Value + 1) % 10;
            });
        }

        public override void Dispose()
        {
            base.Dispose();

            _updateTimer.Dispose();
        }

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
                        .Concat(BuildTreeRows(_model.Tree))
                        .Append(BuildButtonsRow(_model.Roots))
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