using System;
using System.Collections.Generic;
using System.Linq;
using UniMob.UI.Widgets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniMob.UI.Samples.NumTree
{
    public class NumTreeApp : MonoBehaviour
    {
        [SerializeField] private ViewPanel root = default;

        private readonly NumTreeModel _model = new NumTreeModel(levels: 11);

        private IDisposable _render;

        private void OnEnable()
        {
            _render = UniMobUI.RunApp(root, BuildApp);
        }

        private void OnDisable()
        {
            _render.Dispose();
        }

        private void Update()
        {
            var rootIndex = Random.Range(0, _model.Roots.Length);
            var rootAtom = _model.Roots[rootIndex];
            rootAtom.Value = (rootAtom.Value + 1) % 10;
        }

        private Widget BuildApp(BuildContext context)
        {
            return new Container(
                color: Color.white,
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