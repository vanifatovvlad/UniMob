using System;

namespace UniMob.Async
{
    public partial class Future<T>
    {        
        /// <summary>
        /// Register a function to be called when this future completes.
        /// </summary>
        public Future<T> WhenComplete(Action action)
        {
            var future = new Future<T>();
            RegisterContinuation(val =>
            {
                try
                {
                    action();

                    if (Value.IsException)
                    {
                        future.CompleteException(Value.Exception);
                    }
                    else
                    {
                        future.Complete(Value.Value);
                    }
                }
                catch (Exception ex)
                {
                    future.CompleteException(ex);
                }                
            });
            return future;
        }

        /// <summary>
        /// Register a function to be called when this future completes.
        /// </summary>
        public Future<T> WhenComplete(Func<Future<AsyncUnit>> action)
        {
            var future = new Future<T>();
            RegisterContinuation(_ =>
            {
                try
                {
                    
                    var wait = action();
                    wait.TransitTo(val =>
                    {
                        if (Value.IsValue && val.IsValue)
                        {
                            future.Complete(Value.Value);
                        }
                        else
                        {
                            future.CompleteException(Value.Exception ?? val.Exception);
                        }
                    });
                }
                catch (Exception ex)
                {
                    future.CompleteException(ex);
                }
            });
            return future;
        }
    }
}