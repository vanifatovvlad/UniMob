using UniMob.UI.Internal;
using UniMob.UI.Widgets;
using UnityEngine;

[assembly: RegisterComponentViewFactory("$$_FadeTransition",
    typeof(RectTransform), typeof(CanvasGroup), typeof(FadeTransitionView))]

namespace UniMob.UI.Widgets
{
    public class FadeTransitionView : SingleChildLayoutView<IFadeTransitionState>
    {
        private CanvasGroup _canvasGroup;
        private bool _animating;

        protected override void Awake()
        {
            base.Awake();

            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected override void Activate()
        {
            base.Activate();

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
            _canvasGroup.alpha = value;
            _canvasGroup.interactable = completed;
        }
    }

    public interface IFadeTransitionState : ISingleChildLayoutState
    {
        ITween<float> Opacity { get; }
    }
}