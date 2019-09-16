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

        public TweenStatus Status { get; private set; }

        public bool IsCompleted => Status == TweenStatus.Completed;
        public bool IsDismissed => Status == TweenStatus.Dismissed;
        public bool IsAnimating => Status == TweenStatus.Forward || Status == TweenStatus.Reverse;

        public float Value
        {
            get
            {
                var elapsed = Mathf.Clamp01((Time.unscaledTime - _startTime) / Duration);

                switch (Status)
                {
                    case TweenStatus.Forward: return elapsed;
                    case TweenStatus.Reverse: return 1f - elapsed;
                    case TweenStatus.Completed: return 1f;
                    case TweenStatus.Dismissed: return 0f;
                    default: return 0f;
                }
            }
        }

        public ITween<float> View => this;

        public TweenController(float duration)
        {
            if (duration < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(duration), duration, "Must be greater than zero");
            }

            Duration = duration;
            Status = TweenStatus.Dismissed;
        }

        public Task Forward() => StartInternal(TweenStatus.Forward, Duration);

        public Task Reverse() => StartInternal(TweenStatus.Reverse, Duration);

        private Task StartInternal(TweenStatus status, float d)
        {
            _finishTimer?.Dispose();
            _completer?.TrySetResult(null);

            Status = status;
            _startTime = Time.unscaledTime;
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

            _completer.TrySetResult(null);
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