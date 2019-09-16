using System;
using System.Collections.Generic;

namespace UniMob.UI.Internal
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

        private void RenderItems(IState[] states, int startIndex, int count, Action<IView, IState> postRender = null)
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

        private IView RenderItem(IState states)
        {
            if (_activeRender == null)
                throw new InvalidOperationException("Must call BeginRender() before RenderItem()");

            return RenderItemInternal(states).View;
        }

        private Item RenderItemInternal(IState state)
        {
            var nextViewReference = state.View;

            var item = _items.Find(o => ReferenceEquals(o.State, state));
            if (item == null)
            {
                var view = ResolveView(state);
                
                view.SetSource(state);
                item = new Item {State = state, View = view};
                using (Atom.NoWatch)
                {
                    state.DidViewMount();
                }
            }
            else
            {
                _items.Remove(item);

                if (!item.View.ViewReference.Equals(nextViewReference))
                {
                    item.View.ResetSource();
                    RecycleView(item.View);

                    item.View = ResolveView(state);
                }

                item.View.SetSource(state);
                item.State = state;
            }

            item.View.ViewReference.LinkAtomToScope();

            _next.Add(item);

            return item;
        }

        public struct ViewMapperRenderScope : IDisposable
        {
            private readonly ViewMapperBase _mapper;

            public ViewMapperRenderScope(ViewMapperBase mapper)
            {
                _mapper = mapper;
                _mapper.BeginRender();
            }

            void IDisposable.Dispose() => _mapper.EndRender();

            public void RenderItems(IState[] states, Action<IView, IState> postRender = null)
                => _mapper.RenderItems(states, 0, states.Length, postRender);

            public void RenderItems(IState[] states, int startIndex, int count, Action<IView, IState> postRender = null)
                => _mapper.RenderItems(states, startIndex, count, postRender);

            public IView RenderItem(IState state)
                => _mapper.RenderItem(state);
        }

        public ViewMapperRenderScope CreateRender() => new ViewMapperRenderScope(this);

        private void BeginRender()
        {
            if (_activeRender != null)
                throw new InvalidOperationException("Must not call Render() inside other Render()");

            _renderScope.Link(this);
            _activeRender = _renderScope.Enter(this);

            PrepareRender();
        }

        private void EndRender()
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

            using (Atom.NoWatch)
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