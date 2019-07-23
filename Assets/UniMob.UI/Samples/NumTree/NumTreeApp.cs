using System;
using System.Collections.Generic;
using System.Linq;
using UniMob.Async;
using UniMob.UI.Widgets;
using UnityEngine;

namespace UniMob.UI.Samples.NumTree
{
    public class NumTreeApp : MonoBehaviour
    {
        [SerializeField] private ViewPanel root = default;

        private IDisposable _render;

        private void OnEnable() => _render = UniMobUI.RunApp(root, BuildApp);
        private void OnDisable() => _render.Dispose();

        private static MutableAtom<int>[] GenerateTreeRoots(int levels)
        {
            return Enumerable.Repeat(0, levels).Select(v => Atom.Value(v)).ToArray();
        }

        private static Atom<int>[][] GenerateTree(int levels, MutableAtom<int>[] roots)
        {
            var grid = new Atom<int>[levels][];

            // first line - mutable values
            grid[levels - 1] = roots;

            // other - computed from previous line
            for (int level = levels - 2; level >= 0; --level)
            {
                var prevLevel = level + 1;

                var line = Enumerable.Range(0, level + 1).Select(index =>
                    {
                        return Atom.Computed(() =>
                        {
                            var left = grid[prevLevel][index];
                            var right = grid[prevLevel][index + 1];
                            return left.Value + right.Value;
                        });
                    })
                    .ToArray();

                grid[level] = line;
            }

            return grid;
        }

        private Widget BuildApp(BuildContext context)
        {
            const int levels = 11;

            var roots = GenerateTreeRoots(levels);
            var grid = GenerateTree(levels, roots);

            return new Container(
                color: Color.white,
                child: new Column(
                    mainAxisSize: AxisSize.Max,
                    crossAxisSize: AxisSize.Max,
                    mainAxisAlignment: MainAxisAlignment.Center,
                    crossAxisAlignment: CrossAxisAlignment.Center,
                    children: Enumerable.Empty<Widget>()
                        .Concat(BuildTreeRows(grid))
                        .Append(BuildButtonsRow(roots))
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