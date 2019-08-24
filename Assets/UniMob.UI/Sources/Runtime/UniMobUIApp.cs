using System;
using UniMob.UI.Widgets;
using UnityEngine;

namespace UniMob.UI
{
    [AddComponentMenu("UniMob UI/App")]
    public class UniMobUIApp : MonoBehaviour
    {
        [SerializeField] private ScriptableStatefulWidget widget = default;
        [SerializeField] private ViewPanel root = default;

        private IDisposable _render;

        private void OnEnable()
        {
            _render = UniMobUI.RunApp(root, context => widget);
        }

        private void OnDisable()
        {
            _render.Dispose();
        }
    }
}