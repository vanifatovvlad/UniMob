using System;

namespace UniMob.Async
{
    public partial class Future<T>
    {
        /// <summary>
        /// Register callbacks to be called when this future completes.
        /// </summary>
        public Future<T> Then(Action onValue)
        {
            var future = new Future<T>();
            RegisterContinuation(val =>
            {
                if (val.IsException)
                {
                    future.CompleteException(val.Exception);
                    return;
                }

                try
                {
                    onValue();
                    future.Complete(val.Value);
                }
                catch (Exception ex)
                {
                    future.CompleteException(ex);
                }
            });
            return future;
        }

        /// <summary>
        /// Register callbacks to be called when this future completes.
        /// </summary>
        public Future<T> Then(Action<T> onValue)
        {
            var future = new Future<T>();
            RegisterContinuation(val =>
            {
                if (val.IsException)
                {
                    future.CompleteException(val.Exception);
                    return;
                }

                try
                {
                    onValue(val.Value);
                    future.Complete(val.Value);
                }
                catch (Exception ex)
                {
                    future.CompleteException(ex);
                }
            });
            return future;
        }

        /// <summary>
        /// Register callbacks to be called when this future completes.
        /// </summary>
        public Future<TU> Then<TU>(Func<T, TU> onValue)
        {
            var future = new Future<TU>();
            RegisterContinuation(val =>
            {
                if (val.IsValue)
                {
                    try
                    {
                        var result = Future.Value(onValue(val.Value));
                        result.TransitTo(future);
                    }
                    catch (Exception ex)
                    {
                        future.CompleteException(ex);
                    }
                }
                else
                {
                    future.CompleteException(val.Exception);
                }
            });
            return future;
        }

        /// <summary>
        /// Register callbacks to be called when this future completes.
        /// </summary>
        public Future<TU> Then<TU>(Func<T, Future<TU>> onValue)
        {
            var future = new Future<TU>();
            RegisterContinuation(val =>
            {
                if (val.IsValue)
                {
                    try
                    {
                        onValue(val.Value).TransitTo(future);
                    }
                    catch (Exception ex)
                    {
                        future.CompleteException(ex);
                    }
                }
                else
                {
                    future.CompleteException(val.Exception);
                }
            });
            return future;
        }

        /// <summary>
        /// Register callbacks to be called when this future completes.
        /// </summary>
        public Future<TU> Then<TU>(Func<TU> onValue)
        {
            var future = new Future<TU>();
            RegisterContinuation(val =>
            {
                if (val.IsValue)
                {
                    try
                    {
                        Future.Value(onValue()).TransitTo(future);
                    }
                    catch (Exception ex)
                    {
                        future.CompleteException(ex);
                    }
                }
                else
                {
                    future.CompleteException(val.Exception);
                }
            });
            return future;
        }

        /// <summary>
        /// Register callbacks to be called when this future completes.
        /// </summary>
        public Future<TU> Then<TU>(Func<Future<TU>> onValue)
        {
            var future = new Future<TU>();
            RegisterContinuation(val =>
            {
                if (val.IsValue)
                {
                    try
                    {
                        onValue().TransitTo(future);
                    }
                    catch (Exception ex)
                    {
                        future.CompleteException(ex);
                    }
                }
                else
                {
                    future.CompleteException(val.Exception);
                }
            });
            return future;
        }
    }
}