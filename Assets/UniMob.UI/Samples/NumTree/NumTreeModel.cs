using System;
using System.Linq;
using UnityEngine;

namespace UniMob.UI.Samples.NumTree
{
    public class NumTreeModel
    {
        public MutableAtom<int>[] Roots { get; }
        public Atom<int>[][] Tree { get; }

        public NumTreeModel(int levels)
        {
            Roots = GenerateTreeRoots(levels);
            Tree = GenerateTree(levels, Roots);
        }

        private static MutableAtom<int>[] GenerateTreeRoots(int levels)
        {
            var roots = new MutableAtom<int>[levels];
            for (var index = 0; index < roots.Length; index++)
            {
                roots[index] = Atom.Value(0);
            }

            return roots;
        }

        private static Atom<int>[][] GenerateTree(int levels, MutableAtom<int>[] roots)
        {
            var grid = new Atom<int>[levels][];

            // first line - mutable values
            grid[levels - 1] = Array.ConvertAll(roots, root => (Atom<int>) root);

            // other - computed from previous line
            for (var level = levels - 2; level >= 0; --level)
            {
                grid[level] = new Atom<int>[level + 1];

                for (int i = 0; i < level + 1; i++)
                {
                    var prevLevel = level + 1;
                    var index = i;

                    grid[level][index] = Atom.Computed(() =>
                    {
                        var left = grid[prevLevel][index];
                        var right = grid[prevLevel][index + 1];
                        return left.Value + right.Value;
                    });
                }
            }

            return grid;
        }
    }
}