using System;

namespace UniMob.UI
{
    public sealed class ViewRenderScope
    {
        private readonly Scope _scope = new Scope();

        public void Link(IViewTreeElement self)
        {
            _scope.Link(self);
        }

        public IDisposable Enter(IViewTreeElement self)
        {
            _scope.Enter(self);
            return _scope;
        }

        private class Scope : IDisposable
        {
            private bool _linkedFirstRender;
            private int _linkedChildIndex;

            private IViewTreeElement _prevElement;
            private bool _prevFirstRender;
            private int _prevChildIndex;

            public void Link(IViewTreeElement self)
            {
                if (ViewContext.CurrentElement != null)
                    ViewContext.CurrentElement.AddChild(self);

                _linkedFirstRender = ViewContext.FirstRender;
                _linkedChildIndex = ViewContext.ChildIndex;
            }

            public void Enter(IViewTreeElement self)
            {
                _prevElement = ViewContext.CurrentElement;
                ViewContext.CurrentElement = self;

                _prevFirstRender = ViewContext.FirstRender;
                _prevChildIndex = ViewContext.ChildIndex;
                ViewContext.FirstRender = _linkedFirstRender;
                ViewContext.ChildIndex = _linkedChildIndex;
            }

            public void Dispose()
            {
                _linkedFirstRender = false;
                ViewContext.FirstRender = _prevFirstRender;
                ViewContext.ChildIndex = _prevChildIndex;

                ViewContext.CurrentElement = _prevElement;
            }
        }
    }
}