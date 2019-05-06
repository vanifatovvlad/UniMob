using UniMob.Async;
using UniMob.Pools;
using UnityEngine;
using UnityEngine.Assertions;

namespace UniMob.ReView
{
    public class ViewContainer : ViewBase, IView
    {
        private ReactionAtom _renderAtom;

        private bool _hasSource;
        private readonly MutableAtom<Atom<IState>> _childState = Atom.Value(default(Atom<IState>));
        private IState _oldChildState;
        private IView _childView;

        public void SetSource(Atom<IState> source)
        {
            RenderScope.Link(this);

            _hasSource = true;
            _childState.Value = source;

            if (_renderAtom == null)
                _renderAtom = Atom.Reaction(Render);

            _renderAtom.Update();
        }

        protected override void Unmount()
        {
            _hasSource = false;
            _renderAtom.Deactivate();

            base.Unmount();

            UnmountChild();
        }

        private void UnmountChild()
        {
            if (_childView != null)
            {
                _childView.ResetSource();
                GameObjectPool.Recycle(_childView.gameObject);
                _childView = null;
            }

            if (_oldChildState != null)
            {
                _oldChildState.DidViewUnmount();
                _oldChildState = null;
            }
        }

        private void Render()
        {
            var state = _childState.Value.Value;

            if (_oldChildState == state)
                return;

            UnmountChild();

            _oldChildState = state;

            var viewPrefab = ViewContext.Loader.LoadViewPrefab(state, state.ViewPath);
            _childView = GameObjectPool.Instantiate(viewPrefab.gameObject, transform, false)
                .GetComponent<IView>();

            using (RenderScope.Enter(this))
            {
                if (isActiveAndEnabled && gameObject.activeSelf)
                {
                    Children.Clear();
                    _childView.SetSource(new Atom<IState>(state));

                    _oldChildState.DidViewMount();
                }
            }
        }

        void IView.ResetSource()
        {
            Unmount();
        }

        public Vector2 CalcSize(IState model)
        {
            return rectTransform.rect.size;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_hasSource)
            {
                _renderAtom.Update();
            }
        }
    }
}