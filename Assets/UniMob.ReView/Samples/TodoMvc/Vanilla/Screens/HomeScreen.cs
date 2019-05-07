using UniMob.Async;
using UniMob.ReView.Samples.TodoMvc.Vanilla.Widgets;
using UniMob.ReView.Samples.TodoMvc.Views;

namespace UniMob.ReView.Samples.TodoMvc.Vanilla
{
    public class HomeScreen : Widget
    {
        public HomeScreen(TodoStore store) : base(GlobalKey.Of<HomeScreen>())
        {
            Store = store;
        }

        public TodoStore Store { get; }

        public override State CreateState() => new HomeScreenState();
    }

    public class HomeScreenState : State<HomeScreen>, IHomeState
    {
        public MutableAtom<VisibilityFilter> ActiveFilter { get; } = Atom.Value(VisibilityFilter.All);
        public Atom<IState> Todos { get; }

        public HomeScreenState() : base("TodoHomeScreen")
        {
            Todos = CreateChild(context =>
            {
                return new Column(new WidgetList
                {
                    new TodoList(),
                    new TodoList(),
                    new TodoList(),
                }, CrossAxisAlignment.Stretch);
            });
        }

        public void AddTodo()
        {
        }
    }
}