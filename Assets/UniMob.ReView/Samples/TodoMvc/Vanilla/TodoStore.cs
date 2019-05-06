using System.Linq;
using UniMob.Async;

namespace UniMob.ReView.Samples.TodoMvc.Vanilla
{
    public class TodoStore
    {
        private readonly MutableAtom<Todo[]> _todos = Atom.Value(new Todo[0]);

        public Atom<bool> AllCompleted => _todos.Value.All(todo => todo.Completed.Value);
        public Atom<bool> HasCompletedTodos => _todos.Value.Any(todo => todo.Completed.Value);

        public Atom<int> NumActive => _todos.Value.Aggregate(0, (sum, todo) => !todo.Completed.Value ? ++sum : sum);
        public Atom<int> NumCompleted => _todos.Value.Aggregate(0, (sum, todo) => todo.Completed.Value ? ++sum : sum);
/*
        public Atom<Todo[]> FilteredTodos(VisibilityFilter activeFilter) => _todos.Value
            .Where(todo =>
            {
                switch (activeFilter)
                {
                    case VisibilityFilter.Active: return !todo.Completed.Value;
                    case VisibilityFilter.Completed: return todo.Completed.Value;
                    default: return true;
                }
            })
            .ToArray();
*/
        public void AddTodo(Todo todo)
        {
            _todos.Value = _todos.Value.Append(todo).ToArray();
        }

        public void ClearCompleted()
        {
            _todos.Value = _todos.Value.Where(todo => !todo.Completed.Value).ToArray();
        }

        public void ToggleAll()
        {
            var allCompleted = AllCompleted;
            foreach (var todo in _todos.Value)
            {
                todo.Completed.Value = !allCompleted;
            }
        }
    }

    public class Todo
    {
        public MutableAtom<bool> Completed { get; }
        public string Id { get; }
        public string Title { get; }

        public Todo(string id)
        {
            Id = id;
            Completed = Atom.Value(false);
        }
    }
}