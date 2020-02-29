using UniMob.UI.Widgets;
using UnityEngine;

namespace UniMob.UI.Samples.Animation
{
    public class AnimationApp : UniMobUIApp
    {
        [SerializeField] private float duration = 1f;
        [SerializeField] private FloatTween opacityTween = new FloatTween(0, 1);
        [SerializeField] private Vector2Tween positionTween = new Vector2Tween(Vector2.up, Vector2.down);

        [SerializeField] private QuaternionTween rotationTween =
            new QuaternionTween(Quaternion.identity, Quaternion.AngleAxis(90f, Vector3.forward));

        private AnimationController _controller;
        private IAnimation<float> _opacity;
        private IAnimation<Vector2> _position;
        private IAnimation<Quaternion> _rotation;

        protected override void Initialize()
        {
            _controller = new AnimationController(duration);
            _controller.AddStatusListener(OnControllerStatusChanged);

            _opacity = opacityTween.Animate(_controller);
            _position = positionTween.Animate(_controller);
            _rotation = rotationTween.Animate(_controller);

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
                    opacity: _opacity,
                    position: _position,
                    rotation: _rotation,
                    child: new Container(
                        size: WidgetSize.Fixed(300, 200),
                        backgroundColor: Color.black
                    )
                )
            );
        }
    }
}