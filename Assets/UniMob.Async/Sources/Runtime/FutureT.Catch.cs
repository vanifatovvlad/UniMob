using System;

namespace UniMob.Async
{
    public partial class Future<T>
    {
        /// <summary>
        /// Handles errors emitted by this Future.
        /// </summary>
        public Future<AsyncUnit> Catch(Action<Exception> onException, Predicate<Exception> test = null)
        {
            var future = new Future<AsyncUnit>();
            RegisterContinuation(val =>
            {
                if (val.IsException && (test == null || test(val.Exception)))
                {
                    Future.Sync(() => onException(val.Exception)).TransitTo(future);
                }
                else
                {
                    if (val.State == ResultState.Value)
                    {
                        future.Complete(AsyncUnit.Unit);
                    }
                    else
                    {
                        future.CompleteException(val.Exception);
                    }
                }
            });
            return future;
        }

        /// <summary>
        /// Handles errors emitted by this Future.
        /// </summary>
        public Future<AsyncUnit> Catch<TException>(Action<TException> onException, Predicate<TException> test = null)
            where TException : Exception
        {
            var future = new Future<AsyncUnit>();
            RegisterContinuation(val =>
            {
                if (val.IsException && val.Exception is TException tex && (test == null || test(tex)))
                {
                    Future.Sync(() => onException(tex)).TransitTo(future);
                }
                else
                {                    
                    if (val.State == ResultState.Value)
                    {
                        future.Complete(AsyncUnit.Unit);
                    }
                    else
                    {
                        future.CompleteException(val.Exception);
                    }
                }
            });
            return future;
        }

        /// <summary>
        /// Handles errors emitted by this Future.
        /// </summary>
        public Future<T> Catch(Func<Exception, T> onException, Predicate<Exception> test = null)
        {
            var future = new Future<T>();
            RegisterContinuation(val =>
            {
                if (val.IsException && (test == null || test(val.Exception)))
                {
                    Future.Sync(() => onException(val.Exception)).TransitTo(future);
                }
                else
                {
                    TransitTo(future);
                }
            });
            return future;
        }
        
        /// <summary>
        /// Handles errors emitted by this Future.
        /// </summary>
        public Future<T> Catch(Func<Exception, Future<T>> onException, Predicate<Exception> test = null)
        {
            var future = new Future<T>();
            RegisterContinuation(val =>
            {
                if (val.IsException && (test == null || test(val.Exception)))
                {
                    Future.Sync(() => onException(val.Exception)).TransitTo(future);
                }
                else
                {
                    TransitTo(future);
                }
            });
            return future;
        }

        /// <summary>
        /// Handles errors emitted by this Future.
        /// </summary>
        public Future<T> Catch<TException>(Func<TException, T> onException, Predicate<TException> test = null)
            where TException : Exception
        {
            var future = new Future<T>();
            RegisterContinuation(val =>
            {
                if (val.IsException && val.Exception is TException tex && (test == null || test(tex)))
                {
                    Future.Sync(() => onException(tex)).TransitTo(future);
                }
                else
                {
                    TransitTo(future);
                }
            });
            return future;
        }
        
        /// <summary>
        /// Handles errors emitted by this Future.
        /// </summary>
        public Future<T> Catch<TException>(Func<TException, Future<T>> onException, Predicate<TException> test = null)
            where TException : Exception
        {
            var future = new Future<T>();
            RegisterContinuation(val =>
            {
                if (val.IsException && val.Exception is TException tex && (test == null || test(tex)))
                {
                    Future.Sync(() => onException(tex)).TransitTo(future);
                }
                else
                {
                    TransitTo(future);
                }
            });
            return future;
        }
    }
}