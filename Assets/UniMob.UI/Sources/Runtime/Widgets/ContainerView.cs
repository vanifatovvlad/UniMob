using UnityEngine;
using UnityEngine.UI;

namespace UniMob.UI.Widgets
{
    internal sealed class ContainerView : SingleChildLayoutView<IContainerState>
    {
        private Image _backgroundImage;

        protected override void Awake()
        {
            base.Awake();

            _backgroundImage = GetComponent<Image>();
        }

        protected override void Render()
        {
            var backgroundColor = State.BackgroundColor;
            var transparent = backgroundColor == Color.clear;

            _backgroundImage.enabled = !transparent;
            _backgroundImage.color = backgroundColor;

            base.Render();
        }
    }

    public interface IContainerState : ISingleChildLayoutState
    {
        Color BackgroundColor { get; }
    }
}