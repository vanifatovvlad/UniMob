using UniMob.Async;
using UniMob.ReView.Samples.TodoMvc.Views;

namespace UniMob.ReView.Samples.TodoMvc.Vanilla.Widgets
{
    public class TodoList : Widget
    {
        public override State CreateState() => new TodoListState();
    }

    public class TodoListState : State<TodoList>, ITodoListState
    {
        private readonly MutableAtom<bool> _full = Atom.Value(false);

        public TodoListState() : base("TodoList")
        {
        }

        public override WidgetSize CalculateInnerSize()
        {
            var size = base.CalculateInnerSize();
            if (_full.Value) size = new WidgetSize(250, 250);
            return size;
        }

        public void OnTap()
        {
            _full.Value = !_full.Value;
        }
    }
}