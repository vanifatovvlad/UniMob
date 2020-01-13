using System.Collections.Generic;

namespace UniMob.UI
{
    using System;
    using System.Threading.Tasks;
    using UnityEngine;

    public class AnimationController : IAnimation<float>
    {
        private List<Action<AnimationStatus>> _listeners;
        private TaskCompletionSource<object> _completer;

        private Timer _finishTimer;
        private float _startTime;

        public float Duration { get; set; }
        public float ReverseDuration { get; set; }

        public AnimationStatus Status { get; private set; }

        public bool IsCompleted => Status == AnimationStatus.Completed;
        public bool IsDismissed => Status == AnimationStatus.Dismissed;

        public bool IsAnimating =>
            Status == AnimationStatus.Forward || Status == AnimationStatus.Reverse;

        public float Value
        {
            get
            {
                var elapsed = Time.unscaledTime - _startTime;

                switch (Status)
                {
                    case AnimationStatus.Forward: return Mathf.Clamp01(elapsed / Duration);
                    case AnimationStatus.Reverse: return Mathf.Clamp01(1f - elapsed / ReverseDuration);
                    case AnimationStatus.Completed: return 1f;
                    case AnimationStatus.Dismissed: return 0f;
                    default: return 0f;
                }
            }
        }

        public IAnimation<float> View => this;

        public AnimationController(float duration, float? reverseDuration = null, bool completed = false)
        {
            if (duration < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(duration), duration, "Must be positive or zero");
            }

            if (reverseDuration.HasValue && reverseDuration < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(reverseDuration), reverseDuration,
                    "Must positive or zero");
            }

            Duration = duration;
            ReverseDuration = reverseDuration ?? duration;
            Status = completed ? AnimationStatus.Completed : AnimationStatus.Dismissed;
        }

        public void AddStatusListener(Action<AnimationStatus> listener)
        {
            if (_listeners == null)
            {
                _listeners = new List<Action<AnimationStatus>>();
            }

            _listeners.Add(listener);
        }

        public void RemoveStatusListener(Action<AnimationStatus> listener)
        {
            _listeners?.Remove(listener);
        }

        public Task Forward() => StartInternal(AnimationStatus.Forward, Duration);

        public Task Reverse() => StartInternal(AnimationStatus.Reverse, ReverseDuration);

        private Task StartInternal(AnimationStatus status, float d)
        {
            _finishTimer?.Dispose();
            _finishTimer = null;

            _completer?.TrySetResult(null);
            _completer = null;

            _startTime = Time.unscaledTime;
            UpdateStatus(status);

            if (Mathf.Approximately(d, 0))
            {
                OnFinished();
                return Task.CompletedTask;
            }

            _finishTimer = Timer.Delayed(d, OnFinished);
            _completer = new TaskCompletionSource<object>();
            return _completer.Task;
        }

        private void OnFinished()
        {
            switch (Status)
            {
                case AnimationStatus.Forward:
                    Status = AnimationStatus.Completed;
                    UpdateStatus(AnimationStatus.Completed);
                    break;

                case AnimationStatus.Reverse:
                    Status = AnimationStatus.Dismissed;
                    UpdateStatus(AnimationStatus.Dismissed);
                    break;
            }

            _completer?.TrySetResult(null);
        }

        private void UpdateStatus(AnimationStatus status)
        {
            Status = status;

            if (_listeners != null)
            {
                foreach (var listener in _listeners)
                {
                    listener.Invoke(status);
                }
            }
        }
    }

    internal class DrivenAnimation<T> : IAnimation<T>
    {
        private readonly IAnimation<float> _controller;
        private readonly IAnimatable<T> _tween;

        public DrivenAnimation(IAnimation<float> controller, IAnimatable<T> tween)
        {
            _controller = controller;
            _tween = tween;
        }

        public bool IsAnimating => _controller.IsAnimating;
        public bool IsCompleted => _controller.IsCompleted;
        public bool IsDismissed => _controller.IsDismissed;
        public AnimationStatus Status => _controller.Status;
        public T Value => _tween.Transform(_controller.Value);

        public void AddStatusListener(Action<AnimationStatus> listener)
            => _controller.AddStatusListener(listener);

        public void RemoveStatusListener(Action<AnimationStatus> listener) =>
            _controller.RemoveStatusListener(listener);
    }

    public static class AnimatableExtensions
    {
        public static IAnimation<T> Drive<T>(this IAnimation<float> controller,
            IAnimatable<T> tween)
        {
            return tween.Animate(controller);
        }

        public static IAnimation<T> Animate<T>(this IAnimatable<T> parent,
            IAnimation<float> controller)
        {
            return new DrivenAnimation<T>(controller, parent);
        }
    }

    public interface IAnimatable<out T>
    {
        T Transform(float t);
    }

    public interface IAnimation<out T>
    {
        bool IsAnimating { get; }
        bool IsCompleted { get; }
        bool IsDismissed { get; }

        AnimationStatus Status { get; }

        T Value { get; }

        void AddStatusListener(Action<AnimationStatus> listener);
        void RemoveStatusListener(Action<AnimationStatus> listener);
    }

    public enum AnimationStatus
    {
        Dismissed = 0,
        Forward = 1,
        Reverse = 2,
        Completed = 3,
    }
}