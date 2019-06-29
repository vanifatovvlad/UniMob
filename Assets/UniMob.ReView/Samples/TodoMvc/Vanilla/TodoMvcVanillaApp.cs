using System;
using UniMob.ReView.Widgets;
using UnityEngine;

namespace UniMob.ReView.Samples.TodoMvc.Vanilla
{
    public class TodoMvcVanillaApp : MonoBehaviour
    {
        [SerializeField] private ViewPanel root;

        private readonly TodoStore _store = new TodoStore();

        private IDisposable _render;
        
        private void OnEnable()
        {
            _render = ReView.RunApp(root, context =>
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