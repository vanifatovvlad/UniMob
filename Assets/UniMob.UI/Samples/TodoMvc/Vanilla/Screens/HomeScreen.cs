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

        private readonly MutableAtom<CrossFadeState> _crossFade = Atom.Value(CrossFadeState.ShowFirst);

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
            _crossFade.Value = _crossFade.Value == CrossFadeState.ShowFirst
                ? CrossFadeState.ShowSecond
                : CrossFadeState.ShowFirst;
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
                    new Row(
                        children: new List<Widget>
                        {
                            new Container(
                                size: WidgetSize.Fixed(100, 100),
                                backgroundColor: Color.black
                            ),
                            new AnimatedCrossFade(
                                firstChild: new Container(
                                    size: WidgetSize.Fixed(100, 100),
                                    backgroundColor: Color.green
                                ),
                                secondChild: new Container(
                                    size: WidgetSize.FixedWidth(200),
                                    backgroundColor: Color.red
                                ),
                                crossFadeState: _crossFade.Value,
                                duration: 0.5f
                            ),
                            new Container(
                                size: WidgetSize.Fixed(100, 100),
                                backgroundColor: Color.black
                            ),
                        })
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