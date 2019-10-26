using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UniMob.Samples.TodoSample
{
    public class TodoSample : MonoBehaviour
    {
        [SerializeField] private Button addTodoButton = default;
        [SerializeField] private Text unfinishedTodoCountText = default;

        private readonly TodoList _todoList = new TodoList();

        private IDisposable _reaction;

        private void OnEnable()
        {
            _reaction = Atom.AutoRun(() =>
            {
                Debug.Log($"Tasks left: {_todoList.UnfinishedTodoCount}");
                unfinishedTodoCountText.text = $"Tasks left: {_todoList.UnfinishedTodoCount}";
            });

            addTodoButton.onClick.AddListener(AddTodo);
        }

        private void OnDisable()
        {
            addTodoButton.onClick.RemoveListener(AddTodo);
            _reaction.Dispose();
        }

        public void AddTodo()
        {
            _todoList.Todos = _todoList.Todos
                .Append(new Todo {Title = "Get coffee"})
                .ToArray();
        }
    }

    public class Todo
    {
        [Atom] public string Title { get; set; } = "";
        [Atom] public bool Finished { get; set; } = false;
    }

    public class TodoList
    {
        [Atom] public Todo[] Todos { get; set; } = new Todo[0];

        [Atom] public int UnfinishedTodoCount => Todos.Count(t => !t.Finished);
    }
}