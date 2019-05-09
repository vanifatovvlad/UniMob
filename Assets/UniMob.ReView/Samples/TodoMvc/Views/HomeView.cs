using UniMob.Async;
using UnityEngine;
using UnityEngine.UI;

namespace UniMob.ReView.Samples.TodoMvc.Views
{
    public class HomeView : View<IHomeState>
    {
        [SerializeField] private ViewContainer todosContainer;

        [SerializeField] private Button addButton;

        [SerializeField] private Button allVisibleButton;
        [SerializeField] private Button activeVisibleButton;
        [SerializeField] private Button completedVisibleButton;

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
            
            todosContainer.SetSource(State.Todos);
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