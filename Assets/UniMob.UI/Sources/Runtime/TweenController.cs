namespace UniMob.UI
{
    using System;
    using System.Threading.Tasks;
    using UnityEngine;

    public class TweenController : ITween<float>
    {
        private TaskCompletionSource<object> _completer;

        private Timer _finishTimer;
        private float _startTime;

        public float Duration { get; }
        public float ReverseDuration { get; }

        public TweenStatus Status { get; private set; }

        public bool IsCompleted => Status == TweenStatus.Completed;
        public bool IsDismissed => Status == TweenStatus.Dismissed;
        public bool IsAnimating => Status == TweenStatus.Forward || Status == TweenStatus.Reverse;

        public float Value
        {
            get
            {
                var elapsed = Time.unscaledTime - _startTime;

                switch (Status)
                {
                    case TweenStatus.Forward: return Mathf.Clamp01(elapsed / Duration);
                    case TweenStatus.Reverse: return Mathf.Clamp01(1f - elapsed / ReverseDuration);
                    case TweenStatus.Completed: return 1f;
                    case TweenStatus.Dismissed: return 0f;
                    default: return 0f;
                }
            }
        }

        public ITween<float> View => this;

        public TweenController(float duration, float? reverseDuration = null)
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
            Status = TweenStatus.Dismissed;
        }

        public Task Forward() => StartInternal(TweenStatus.Forward, Duration);

        public Task Reverse() => StartInternal(TweenStatus.Reverse, ReverseDuration);

        private Task StartInternal(TweenStatus status, float d)
        {
            _finishTimer?.Dispose();
            _finishTimer = null;

            _completer?.TrySetResult(null);
            _completer = null;

            Status = status;
            _startTime = Time.unscaledTime;

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
                case TweenStatus.Forward:
                    Status = TweenStatus.Completed;
                    break;

                case TweenStatus.Reverse:
                    Status = TweenStatus.Dismissed;
                    break;
            }

            _completer?.TrySetResult(null);
        }
    }

    public interface ITween<T>
    {
        bool IsAnimating { get; }
        bool IsCompleted { get; }
        bool IsDismissed { get; }

        TweenStatus Status { get; }

        T Value { get; }
    }

    public enum TweenStatus
    {
        Dismissed = 0,
        Forward = 1,
        Reverse = 2,
        Completed = 3,
    }
}