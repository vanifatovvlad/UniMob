using System;
using UniMob.ReView.Widgets;
using UnityEngine;

namespace UniMob.ReView.Samples.Counter
{
    public class CounterApp : MonoBehaviour
    {
        [SerializeField] private ViewPanel root = default;

        private IDisposable _render;

        private void OnEnable()
        {
            _render = ReView.RunApp(root, context => new Counter());
        }

        private void OnDisable()
        {
            _render.Dispose();
        }
    }
}