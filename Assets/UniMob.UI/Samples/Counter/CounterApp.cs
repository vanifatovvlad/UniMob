using System;
using UniMob.UI;
using UniMob.UI.Widgets;
using UnityEngine;

namespace UniMob.UI.Samples.Counter
{
    public class CounterApp : MonoBehaviour
    {
        [SerializeField] private ViewPanel root = default;

        private IDisposable _render;

        private void OnEnable()
        {
            _render = UniMobUI.RunApp(root, context => new Counter());
        }

        private void OnDisable()
        {
            _render.Dispose();
        }
    }
}