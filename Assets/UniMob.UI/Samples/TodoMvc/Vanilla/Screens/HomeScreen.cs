using System.Collections.Generic;
using UniMob.UI.Samples.TodoMvc.Vanilla.Widgets;
using UniMob.UI.Samples.TodoMvc.Views;
using UniMob.UI.Widgets;
using UnityEngine;

namespace UniMob.UI.Samples.TodoMvc.Vanilla
{
    public class HomeScreen : StatefulWidget
    {
        public HomeScreen(TodoStore store)
            : base(GlobalKey.Of<HomeScreenState>())
        {
            Store = store;
        }

        public TodoStore Store { get; }

        public override State CreateState() => new HomeScreenState();
    }

    public class HomeScreenState : ViewState<HomeScreen>, IHomeState
    {
        private readonly MutableAtom<VisibilityFilter> _activeFilter = Atom.Value(VisibilityFilter.All);
        private readonly Atom<IState> _todos;

        public override WidgetViewReference View { get; }
            = WidgetViewReference.Resource("TodoHomeScreen");

        public HomeScreenState()
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
            new ZStack(
                children: new List<Widget>
                {
                    new Container(
                        child: new Row(
                            children: new List<Widget>
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
                        backgroundColor: Color.cyan
                    ),
                    new Container(
                        size: WidgetSize.Fixed(100, 100),
                        backgroundColor: Color.green
                    )
                });

        private Widget BuildSubContainer() =>
            new Container(
                child: new Column(
                    children: new List<Widget>
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
                backgroundColor: Color.grey
            );
    }
}