using UnityEngine;

namespace UniMob.ReView.Samples.TodoMvc.Vanilla
{
    public class TodoMvcVanillaApp : MonoBehaviour
    {
        [SerializeField] private ViewContainer root;

        private readonly TodoStore _store = new TodoStore();

        private void Start()
        {
            ReView.RunApp(root, context =>
            {
                //
                return new HomeScreen(store: _store);
            });
        }
    }
}