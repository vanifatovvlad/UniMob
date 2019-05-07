using System;
using UniMob.Async;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace UniMob.ReView
{
    public abstract class View<TState> : ViewBase, IView
        where TState : IState
    {
        private bool _mounted;

        private bool _hasState;
        private TState _state;

        private bool _hasSource;
        private readonly MutableAtom<TState> _source = Atom.Value(default(TState));

        private ReactionAtom _renderAtom;

        public bool HasState => _hasState;

        [NotNull] protected TState State => _state;

        void IView.SetSource(IState newSource)
        {
            if (!(newSource is TState nextState))
            {
                var expected = typeof(TState).Name;
                var actual = newSource.GetType().Name;
                Debug.LogError($"Wrong model type at '{name}': expected={expected}, actual={actual}");
                return;
            }

            RenderScope.Link(this);

            if (_renderAtom == null)
                _renderAtom = Atom.Reaction(RenderAction);

            _hasSource = true;
            _source.Value = nextState;

            _renderAtom.Update();
        }

        protected sealed override void Unmount()
        {
            if (!_hasSource)
            {
                Assert.IsFalse(_hasState, "hasModel");
                Assert.IsFalse(_mounted, "mounted");
                return;
            }

            Assert.IsNotNull(_renderAtom, "renderAtom == null");
            _renderAtom.Deactivate();

            _source.Value = default;
            _hasSource = false;

            if (!_hasState)
            {
                Assert.IsFalse(_mounted, "mounted");
                return;
            }

            try
            {
                using (Atom.NoLinkScope)
                {
                    Deactivate();
                }
            }
            catch (Exception ex)
            {
                Zone.Current.HandleUncaughtException(ex);
            }

            if (_mounted)
            {
                _mounted = false;

                try
                {
                    using (Atom.NoLinkScope)
                    {
                        _state.DidViewUnmount();
                    }
                }
                catch (Exception ex)
                {
                    Zone.Current.HandleUncaughtException(ex);
                }
            }

            base.Unmount();

            _state = default;
            _hasState = false;
        }

        void IView.ResetSource()
        {
            Unmount();
        }

        private void RenderAction()
        {
            Assert.IsTrue(_hasSource, "!hasSource");

            var nextState = _source.Value;
            if (nextState == null)
            {
                Debug.LogWarning("Model == null", this);
                return;
            }

            using (Atom.NoLinkScope)
            {
                if (!_hasState || !nextState.Equals(_state))
                {
                    if (_hasState)
                    {
                        try
                        {
                            Deactivate();
                        }
                        catch (Exception ex)
                        {
                            Zone.Current.HandleUncaughtException(ex);
                        }
                    }

                    _hasState = true;
                    _state = nextState;

                    try
                    {
                        Activate();
                    }
                    catch (Exception ex)
                    {
                        Zone.Current.HandleUncaughtException(ex);
                    }
                }
            }

            Assert.IsNotNull(RenderScope, "renderScope == null");
            using (RenderScope.Enter(this))
            {
                if (isActiveAndEnabled && gameObject.activeSelf)
                {
                    Children.Clear();

                    try
                    {
                        Render();
                    }
                    catch (Exception ex)
                    {
                        Zone.Current.HandleUncaughtException(ex);
                    }

                    if (!_mounted)
                    {
                        _mounted = true;

                        using (Atom.NoLinkScope)
                        {
                            _state.DidViewMount();
                        }
                    }
                }
            }
        }

        Vector2 IView.CalcSize(IState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (!(state is TState stateTyped))
            {
                var expected = typeof(TState).Name;
                var actual = state.GetType().Name;
                throw new ArgumentException($"Unexpected state type: expected={expected} actual={actual}");
            }

            return CalcSize(stateTyped);
        }

        protected virtual Vector2 CalcSize(TState state)
        {
            return rectTransform.rect.size;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_hasSource)
            {
                Assert.IsNotNull(_renderAtom, "renderAtom == null");
                _renderAtom.Update();
            }
        }

        protected virtual void Activate()
        {
        }

        protected virtual void Deactivate()
        {
        }

        protected abstract void Render();
    }

    public interface IState
    {
        string ViewPath { get; }

        void DidViewMount();
        void DidViewUnmount();
    }

    public interface IView
    {
        // ReSharper disable once InconsistentNaming
        GameObject gameObject { get; }

        // ReSharper disable once InconsistentNaming
        RectTransform rectTransform { get; }

        void SetSource(IState source);
        void ResetSource();
        Vector2 CalcSize(IState model);
    }

    public interface IViewTreeElement
    {
        void AddChild(IViewTreeElement view);
        void Unmount();
    }

    public class ViewRenderScope
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

    public static class ViewContext
    {
        internal static IViewTreeElement CurrentElement;
        internal static IViewLoader Loader = new BuiltinResourcesViewLoader();

        public static bool FirstRender;
        public static int ChildIndex;

        public static IDisposable FirstRenderScope() => new FirstRenderScopeStruct(true);

        private struct FirstRenderScopeStruct : IDisposable
        {
            private readonly bool _old;

            public FirstRenderScopeStruct(bool firstRender)
            {
                _old = FirstRender;
                FirstRender = firstRender;
            }

            public void Dispose()
            {
                FirstRender = _old;
            }
        }
    }
}