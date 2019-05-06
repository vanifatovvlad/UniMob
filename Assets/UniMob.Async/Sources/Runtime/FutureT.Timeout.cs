using System;

namespace UniMob.Async
{
    public partial class Future<T>
    {
        /// <summary>
        /// Time-out the future computation after timeLimit has passed.
        /// </summary>
        public Future<AsyncUnit> Timeout(float timeout, Action onTimeout)
        {
            var future = new Future<AsyncUnit>();
            var handled = false;

            var timer = Timer.Delayed(timeout, () =>
            {
                if (Utils.SetBool(ref handled, true))
                {
                    Future.Sync(onTimeout).TransitTo(future);
                }
            });

            RegisterContinuation(val =>
            {
                if (Utils.SetBool(ref handled, true))
                {
                    timer.Dispose();

                    if (Value.IsValue)
                    {
                        future.Complete(AsyncUnit.Unit);
                    }
                    else
                    {
                        future.CompleteException(Value.Exception);
                    }
                }
            });

            return future;
        }

        /// <summary>
        /// Time-out the future computation after timeLimit has passed.
        /// </summary>
        public Future<AsyncUnit> Timeout(TimeSpan timeout, Action onTimeout)
            => Timeout((float) timeout.TotalSeconds, onTimeout);

        /// <summary>
        /// Time-out the future computation after timeLimit has passed.
        /// </summary>
        public Future<T> Timeout(float timeout, Func<Future<T>> onTimeout)
        {
            var future = new Future<T>();
            var handled = false;

            var timer = Timer.Delayed(timeout, () =>
            {
                if (Utils.SetBool(ref handled, true))
                {
                    Future.Sync(onTimeout).TransitTo(future);
                }
            });

            RegisterContinuation(val =>
            {
                if (Utils.SetBool(ref handled, true))
                {
                    timer.Dispose();
                    TransitTo(future);
                }
            });

            return future;
        }

        /// <summary>
        /// Time-out the future computation after timeLimit has passed.
        /// </summary>
        public Future<T> Timeout(TimeSpan timeout, Func<Future<T>> onTimeout)
            => Timeout((float) timeout.TotalSeconds, onTimeout);

        /// <summary>
        /// Time-out the future computation after timeLimit has passed.
        /// </summary>
        public Future<T> Timeout(float timeout, Func<T> onTimeout)
        {
            var future = new Future<T>();
            var handled = false;

            var timer = Timer.Delayed(timeout, () =>
            {
                if (Utils.SetBool(ref handled, true))
                    Future.Sync(onTimeout).TransitTo(future);
            });

            RegisterContinuation(val =>
            {
                if (Utils.SetBool(ref handled, true))
                {
                    timer.Dispose();
                    TransitTo(future);
                }
            });

            return future;
        }

        /// <summary>
        /// Time-out the future computation after timeLimit has passed.
        /// </summary>
        public Future<T> Timeout(TimeSpan timeout, Func<T> onTimeout)
            => Timeout((float) timeout.TotalSeconds, onTimeout);
    }
}