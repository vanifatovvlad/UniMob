using System;

namespace UniMob.Async
{
    /// <summary>
    /// Class that produce Future objects and to complete them later with a value or error.
    /// </summary>
    public class Completer<T>
    {
        private readonly Future<T> _future;
        private bool _mayComplete = true;

        /// <summary>
        /// The future that will contain the result provided to this completer.
        /// </summary>
        public Future<T> Future => _future;

        /// <summary>
        /// Whether the future has been completed.
        /// </summary>
        public bool IsCompleted => !_mayComplete || _future.IsCompleted;

        /// <summary>
        /// Creates a new completer.
        /// </summary>
        public Completer()
        {
            _future = new Future<T>();
        }

        /// <summary>
        /// Completes future with the supplied value.
        /// </summary>
        public void Complete(T value)
        {
            if (!_mayComplete)
            {
                throw new InvalidOperationException(
                    $"Complete(...) must be called only once. Current state: {_future.Value}");
            }

            _mayComplete = false;
            Zone.Current.Invoke(() => _future.Complete(value));
        }

        /// <summary>
        /// Completes future with the supplied value.
        /// </summary>
        public void TryComplete(T value)
        {
            if (!_mayComplete)
                return;

            _mayComplete = false;
            Zone.Current.Invoke(() => _future.Complete(value));
        }

        /// <summary>
        /// Complete future with an error.
        /// </summary>
        public void CompleteException(Exception exception)
        {
            if (!_mayComplete)
            {
                throw new InvalidOperationException(
                    $"Complete(...) must be called only once. Current state: {_future.Value}");
            }

            _mayComplete = false;
            Zone.Current.Invoke(() => _future.CompleteException(exception));
        }

        /// <summary>
        /// Complete future with an error.
        /// </summary>
        public void TryCompleteException(Exception exception)
        {
            if (!_mayComplete)
                return;

            _mayComplete = false;
            Zone.Current.Invoke(() => _future.CompleteException(exception));
        }
    }
}