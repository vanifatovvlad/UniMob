using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UniMob.Async
{
    /// <summary>
    /// An object representing a delayed computation.
    /// </summary>
    public partial class Future<T>
    {
        private Action<Result<T>> _continuation;
        private Result<T>? _result;

        internal Result<T> Value => _result ?? throw new InvalidOperationException();
        internal bool IsCompleted => _result.HasValue;

        internal Future()
        {
        }

        internal Future(Result<T> value) => _result = value;

        internal void Complete(T value) => CompleteResult(Result.Value(value));
        internal void CompleteException(Exception exception) => CompleteResult(Result.Exception<T>(exception));

        internal void CompleteResult(Result<T> result)
        {
            if (_result.HasValue)
                throw new InvalidOperationException("Future already completed");

            _result = result;

            RunContinuation();
        }

        internal void TransitTo(Future<T> future)
        {
            if (!_result.HasValue)
            {
                RegisterContinuation(_ => TransitTo(future));
                return;
            }

            future.CompleteResult(_result.Value);
        }

        internal void TransitTo(Action<Result<T>> continuation)
        {
            if (!_result.HasValue)
            {
                RegisterContinuation(_ => TransitTo(continuation));
                return;
            }

            continuation(_result.Value);
        }

        internal void RegisterContinuation(Action<Result<T>> continuation)
        {
            _continuation += continuation ?? throw new ArgumentNullException(nameof(continuation));

            if (_result.HasValue)
            {
                Zone.Current.Invoke(RunContinuation);
            }
        }

        private void RunContinuation()
        {
            if (!_result.HasValue)
                throw new InvalidOperationException("Must not call RunContinuation in NotReady state");

            _continuation?.Invoke(_result.Value);

            if (_result.Value.IsException && _continuation == null)
            {
                Zone.Current.HandleUncaughtException(_result.Value.Exception);
            }

            _continuation = null;
        }

        public Future<AsyncUnit> ToUnit()
        {
            return Then(() => AsyncUnit.Unit);
        }
    }
}