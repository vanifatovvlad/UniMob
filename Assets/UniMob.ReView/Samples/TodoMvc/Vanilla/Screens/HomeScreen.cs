using UniMob.Async;
using UniMob.ReView.Samples.TodoMvc.Vanilla.Widgets;
using UniMob.ReView.Samples.TodoMvc.Views;
using UniMob.ReView.Widgets;

namespace UniMob.ReView.Samples.TodoMvc.Vanilla
{
    public class HomeScreen : Widget
    {
        public HomeScreen(TodoStore store)
            : base(GlobalKey.Of<HomeScreen>())
        {
            Store = store;
        }

        public TodoStore Store { get; }

        public override State CreateState() => new HomeScreenState();
    }

    public class HomeScreenState : State<HomeScreen>, IHomeState
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
            return new Column(
                children: new WidgetList
                {
                    new TodoList(),
                    new Column(
                        children: new WidgetList
                        {
                            new TodoList(),
                            new TodoList(),
                            new TodoList(),
                        },
                        crossAxisAlignment: (
                            ActiveFilter == VisibilityFilter.All ? CrossAxisAlignment.Start :
                            ActiveFilter == VisibilityFilter.Active ? CrossAxisAlignment.Center :
                            CrossAxisAlignment.End
                        ),
                        stretchHorizontal: true
                    ),
                    new TodoList(),
                },
                crossAxisAlignment: CrossAxisAlignment.Center,
                mainAxisAlignment: (
                    ActiveFilter == VisibilityFilter.All ? MainAxisAlignment.Start :
                    ActiveFilter == VisibilityFilter.Active ? MainAxisAlignment.Center :
                    MainAxisAlignment.End
                ),
                stretchHorizontal: true,
                stretchVertical: true
            );
        }
    }
}