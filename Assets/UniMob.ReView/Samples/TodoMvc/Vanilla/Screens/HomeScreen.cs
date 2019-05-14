using UniMob.Async;
using UniMob.ReView.Samples.TodoMvc.Vanilla.Widgets;
using UniMob.ReView.Samples.TodoMvc.Views;

namespace UniMob.ReView.Samples.TodoMvc.Vanilla
{
    public class HomeScreen : Widget
    {
        public HomeScreen(TodoStore store)
        {
            Store = store;
            Key = GlobalKey.Of<HomeScreen>();
        }

        public TodoStore Store { get; }

        public override State CreateState() => new HomeScreenState();
    }

    public class HomeScreenState : State<HomeScreen>, IHomeState, ILayoutState
    {
        private readonly MutableAtom<VisibilityFilter> _activeFilter = Atom.Value(VisibilityFilter.All);
        private readonly Atom<IState> _todos;

        public HomeScreenState() : base("TodoHomeScreen")
        {
            _todos = CreateChild(BuildTodos);
        }

        public VisibilityFilter ActiveFilter
        {
            get => _activeFilter.Value;
            set => _activeFilter.Value = value;
        }

        public IState Todos => _todos.Value;

        public void AddTodo()
        {
        }

        private Widget BuildTodos(BuildContext context)
        {
            return new Column(new WidgetList
            {
                new TodoList(),
                new Column(new WidgetList
                {
                    new TodoList(),
                    new TodoList(),
                    new TodoList(),
                }) {CrossAxisAlignment = CrossAxisAlignment.Center, StretchHorizontal = true},
                new TodoList(),
            })
            {
                CrossAxisAlignment = CrossAxisAlignment.Start, StretchHorizontal = true, StretchVertical = true,
                MainAxisAlignment = MainAxisAlignment.End
            };
        }

        public bool StretchVertical { get; } = true;
        public bool StretchHorizontal { get; } = true;
    }
}