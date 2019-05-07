using System;
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
        private readonly MutableAtom<IState> _childState = Atom.Value(default(IState));

        private ViewMapperBase _mapper;

        public void SetSource(IState source)
        {
            RenderScope.Link(this);

            _hasSource = true;
            _childState.Value = source;

            if (_mapper == null)
                _mapper = new PooledViewMapper(transform);

            if (_renderAtom == null)
                _renderAtom = Atom.Reaction(Render);

            _renderAtom.Update();
        }

        protected override void Unmount()
        {
            Assert.IsNull(Atom.CurrentScope);

            _hasSource = false;
            _renderAtom.Deactivate();

            base.Unmount();
        }

        private void Render()
        {
            var state = _childState.Value;

            using (RenderScope.Enter(this))
            {
                try
                {
                    _mapper.BeginRender();
                    _mapper.RenderItem(state);
                }
                finally
                {
                    _mapper.EndRender();
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