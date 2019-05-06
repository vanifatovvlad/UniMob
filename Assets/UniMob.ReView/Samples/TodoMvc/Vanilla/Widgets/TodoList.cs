using UniMob.ReView.Samples.TodoMvc.Views;
using JetBrains.Annotations;
using UnityEngine;

namespace UniMob.ReView.Samples.TodoMvc.Vanilla.Widgets
{
    public class TodoList : Widget
    {
        public TodoList([CanBeNull] Key key = null) : base(key)
        {
        }

        public override State CreateState() => new TodoListState();
    }

    public class TodoListState : State<TodoList>, ITodoListState
    {
        public TodoListState() : base("TodoList")
        {
        }

        public override void InitState()
        {
            base.InitState();
            
            Debug.Log("Init List");
        }

        public override void Dispose()
        {
            base.Dispose();
            
            Debug.Log("Dispose List");
        }
    }
}