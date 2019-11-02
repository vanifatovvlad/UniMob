using System;
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
            _render = UniMobUI.RunApp(root, context => new Widgets.NumTree(_model));
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
    }
}