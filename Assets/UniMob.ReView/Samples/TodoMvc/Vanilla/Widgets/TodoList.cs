using JetBrains.Annotations;
using UniMob.ReView.Samples.TodoMvc.Views;

namespace UniMob.ReView.Samples.TodoMvc.Vanilla.Widgets
{
    public class TodoList : Widget
    {
        public override State CreateState() => new TodoListState();
    }

    public class TodoListState : State<TodoList>, ITodoListState
    {
        public TodoListState() : base("TodoList")
        {
        }
    }
}