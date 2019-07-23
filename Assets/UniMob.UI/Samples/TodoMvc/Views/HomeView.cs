using UniMob.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace UniMob.UI.Samples.TodoMvc.Views
{
    public class HomeView : View<IHomeState>
    {
        [SerializeField] private ViewPanel todosContainer = default;

        [SerializeField] private Button addButton = default;

        [SerializeField] private Button allVisibleButton = default;
        [SerializeField] private Button activeVisibleButton = default;
        [SerializeField] private Button completedVisibleButton = default;

        protected override void Awake()
        {
            base.Awake();

            addButton.Click(() => State.AddTodo);

            allVisibleButton.Click(() => State.ActiveFilter = VisibilityFilter.All);
            activeVisibleButton.Click(() => State.ActiveFilter = VisibilityFilter.Active);
            completedVisibleButton.Click(() => State.ActiveFilter = VisibilityFilter.Completed);
        }

        protected override void Render()
        {
            var filter = State.ActiveFilter;
            allVisibleButton.interactable = filter != VisibilityFilter.All;
            activeVisibleButton.interactable = filter != VisibilityFilter.Active;
            completedVisibleButton.interactable = filter != VisibilityFilter.Completed;
            
            todosContainer.SetState(State.Todos);
        }
    }

    public interface IHomeState : IState
    {
        VisibilityFilter ActiveFilter { get; set; }
        IState Todos { get; }

        void AddTodo();
    }


    public enum VisibilityFilter
    {
        All,
        Active,
        Completed,
    }
}