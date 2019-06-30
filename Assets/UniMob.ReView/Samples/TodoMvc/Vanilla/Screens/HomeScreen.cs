using UniMob.Async;
using UniMob.ReView.Samples.TodoMvc.Vanilla.Widgets;
using UniMob.ReView.Samples.TodoMvc.Views;
using UniMob.ReView.Widgets;
using UnityEngine;

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

        private Widget BuildTodos(BuildContext context) =>
            new Container(
                child: new Row(
                    children: new WidgetList
                    {
                        new TodoList(),
                        BuildSubContainer(),
                        new TodoList(),
                    },
                    crossAxisAlignment: CrossAxisAlignment.Center,
                    mainAxisAlignment: (
                        ActiveFilter == VisibilityFilter.All ? MainAxisAlignment.Start :
                        ActiveFilter == VisibilityFilter.Active ? MainAxisAlignment.Center :
                        MainAxisAlignment.End
                    ),
                    crossAxisSize: AxisSize.Max,
                    mainAxisSize: AxisSize.Max
                ),
                color: Color.cyan
            );

        private Widget BuildSubContainer() =>
            new Container(
                child: new Column(
                    children: new WidgetList
                    {
                        new TodoList(),
                        new TodoList(),
                        new TodoList(),
                    },
                    crossAxisAlignment: CrossAxisAlignment.Center,
                    mainAxisAlignment: (
                        ActiveFilter == VisibilityFilter.All ? MainAxisAlignment.Start :
                        ActiveFilter == VisibilityFilter.Active ? MainAxisAlignment.Center :
                        MainAxisAlignment.End
                    ),
                    mainAxisSize: AxisSize.Max
                ),
                color: Color.grey
            );
    }
}