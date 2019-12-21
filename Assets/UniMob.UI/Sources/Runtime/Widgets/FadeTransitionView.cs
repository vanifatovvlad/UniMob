namespace UniMob.UI.Widgets
{
    using Internal;
    using UnityEngine;

    public class FadeTransitionView : View<IFadeTransitionState>
    {
        [SerializeField] private CanvasGroup canvasGroup = default;

        private ViewMapperBase _mapper;
        private bool _animating;

        protected override void Render()
        {
            using (var render = _mapper.CreateRender())
            {
                var child = State.Child;
                var childView = render.RenderItem(child);
                var childSize = child.Size;

                var alignment = Alignment.Center;
                ViewLayoutUtility.SetSize(childView.rectTransform, childSize, alignment.ToAnchor());
                ViewLayoutUtility.SetPosition(childView.rectTransform, childSize, Vector2.zero, alignment);
            }
        }

        protected override void Activate()
        {
            base.Activate();

            if (_mapper == null)
                _mapper = new PooledViewMapper(transform);

            _animating = true;
            UpdateOpacity(State.Opacity.Value, false);
        }

        protected override void Deactivate()
        {
            base.Deactivate();

            _animating = false;
        }

        private void Update()
        {
            if (!HasState)
            {
                return;
            }

            var opacity = State.Opacity;

            if (_animating)
            {
                UpdateOpacity(opacity.Value, opacity.IsCompleted);
            }

            _animating = opacity.IsAnimating;
        }

        private void UpdateOpacity(float value, bool completed)
        {
            canvasGroup.alpha = value;
            canvasGroup.interactable = completed;
        }
    }

    public interface IFadeTransitionState : IViewState
    {
        IState Child { get; }

        ITween<float> Opacity { get; }
    }
}