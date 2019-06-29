using UnityEngine;
using UnityEngine.UI;

namespace UniMob.ReView.Samples.TodoMvc.Views
{
    public class TodoListView : View<ITodoListState>
    {
        [SerializeField] private Button button = default;

        protected override void Awake()
        {
            base.Awake();
            
            button.Click(() => State.OnTap);
        }

        protected override void Render()
        {
            
        }
    }

    public interface ITodoListState : IState
    {
        void OnTap();
    }
}