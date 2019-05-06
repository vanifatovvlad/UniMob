using UniMob.Async;
using UniMob.ReView.Samples.TodoMvc.Vanilla.Widgets;
using UniMob.ReView.Samples.TodoMvc.Views;
using UnityEngine;

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

        private MutableAtom<int> _add = Atom.Value(0);

        public HomeScreenState() : base("TodoHomeScreen")
        {
            var a = new TodoList(Key.Of(1));
            var b = new TodoList(Key.Of(2));

            Todos = CreateChild(context =>
            {
                //
                return _add.Value % 2 == 0 ? a : b;
            });
        }

        public void AddTodo()
        {
            _add.Value++;
        }

        public override void Dispose()
        {
            base.Dispose();

            Debug.Log("Dispose Home");
        }
    }
}