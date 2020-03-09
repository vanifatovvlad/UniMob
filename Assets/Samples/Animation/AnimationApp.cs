using UniMob.UI;
using UniMob.UI.Widgets;
using UnityEngine;

namespace Samples.Animation
{
    public class AnimationApp : UniMobUIApp
    {
        public float duration = 1f;

        public FloatTween opacity = new FloatTween(0, 1);
        public Vector2Tween position = new Vector2Tween(Vector2.up, Vector2.down);

        private AnimationController _controller;

        protected override void Initialize()
        {
            _controller = new AnimationController(duration);
            _controller.AddStatusListener(OnControllerStatusChanged);
            _controller.Forward();
        }

        private void OnControllerStatusChanged(AnimationStatus status)
        {
            switch (status)
            {
                case AnimationStatus.Completed:
                    _controller.Reverse();
                    break;

                case AnimationStatus.Dismissed:
                    _controller.Forward();
                    break;
            }
        }

        protected override Widget Build(BuildContext context)
        {
            return new Container(
                size: WidgetSize.Stretched,
                backgroundColor: Color.white,
                child: new CompositeTransition(
                    opacity: opacity.Animate(_controller),
                    position: position.Animate(_controller),
                    child: new Container(
                        size: WidgetSize.Fixed(300, 200),
                        backgroundColor: Color.black
                    )
                )
            );
        }
    }
}