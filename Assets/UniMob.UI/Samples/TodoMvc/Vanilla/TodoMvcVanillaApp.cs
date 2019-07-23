using System;
using UniMob.UI.Widgets;
using UnityEngine;

namespace UniMob.UI.Samples.TodoMvc.Vanilla
{
    public class TodoMvcVanillaApp : MonoBehaviour
    {
        [SerializeField] private ViewPanel root = default;

        private readonly TodoStore _store = new TodoStore();

        private IDisposable _render;
        
        private void OnEnable()
        {
            _render = UniMobUI.RunApp(root, context =>
            {
                //
                return new HomeScreen(store: _store);
            });
        }

        private void OnDisable()
        {
            _render.Dispose();
        }
    }
}