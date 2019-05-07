using System;
using System.Collections.Generic;
using UniMob.Async;

namespace UniMob.ReView
{
    public abstract class ViewMapperBase : IViewTreeElement
    {
        private List<Item> _items = new List<Item>();
        private List<Item> _next = new List<Item>();

        private readonly ViewRenderScope _renderScope = new ViewRenderScope();
        private IDisposable _activeRender;

        class Item
        {
            public IView View;
            public IState State;
        }

        public int Count => _items.Count;

        protected abstract IView ResolveView(IState state);
        protected abstract void RecycleView(IView view);

        void IViewTreeElement.AddChild(IViewTreeElement view)
        {
        }

        void IViewTreeElement.Unmount()
        {
            RemoveAll();
        }

        public void Render(IState[] states, Action<IView, IState> postRender = null)
        {
            Render(states, 0, states.Length, postRender);
        }

        public void Render(IState[] states, int startIndex, int count, Action<IView, IState> postRender = null)
        {
            BeginRender();
            RenderItems(states, startIndex, count, postRender);
            EndRender();
        }

        public void RenderItems(IState[] states, int startIndex, int count, Action<IView, IState> postRender = null)
        {
            if (states == null)
                throw new ArgumentNullException(nameof(states));

            if (_activeRender == null)
                throw new InvalidOperationException("Must call BeginRender() before RenderArray()");

            for (int i = 0; i < count; i++)
            {
                var model = states[startIndex + i];

                var oldChildIndex = ViewContext.ChildIndex;
                ViewContext.ChildIndex = startIndex + i;

                var item = RenderItemInternal(model);
                postRender?.Invoke(item.View, item.State);

                ViewContext.ChildIndex = oldChildIndex;
            }
        }

        public IView RenderItem(IState states)
        {
            if (_activeRender == null)
                throw new InvalidOperationException("Must call BeginRender() before RenderItem()");

            return RenderItemInternal(states).View;
        }

        private Item RenderItemInternal(IState state)
        {
            var item = _items.Find(o => ReferenceEquals(o.State, state));
            if (item == null)
            {
                var view = ResolveView(state);
                view.SetSource(state);
                item = new Item {State = state, View = view};
                using (Atom.NoLinkScope)
                {
                    state.DidViewMount();
                }
            }
            else
            {
                _items.Remove(item);
                //item.View.SetSource(state);
                //item.State = state;
            }

            _next.Add(item);

            return item;
        }

        public void BeginRender()
        {
            if (_activeRender != null)
                throw new InvalidOperationException("Must not call Render() inside other Render()");

            _renderScope.Link(this);
            _activeRender = _renderScope.Enter(this);

            PrepareRender();
        }

        public void EndRender()
        {
            if (_activeRender == null)
                throw new InvalidOperationException("Must not call EndRender() without BeginRender()");

            RemoveAll();

            var old = _items;
            _items = _next;
            _next = old;

            _activeRender?.Dispose();
            _activeRender = null;
        }

        private void RemoveAll()
        {
            RemoveAllAndClear(_items);
        }

        private void PrepareRender()
        {
            RemoveAllAndClear(_next);
        }

        private void RemoveAllAndClear(List<Item> list)
        {
            if (list.Count == 0)
                return;

            using (Atom.NoLinkScope)
            {
                foreach (var removed in list)
                {
                    removed.View.ResetSource();
                    RecycleView(removed.View);
                    removed.State.DidViewUnmount();
                }

                list.Clear();
            }
        }

        public IView GetViewAt(int index)
        {
            if (index < 0 || index > _items.Count)
                throw new ArgumentNullException(nameof(index));

            return _items[index].View;
        }
    }
}