using System;
using System.Collections.Generic;

namespace UniMob.Async
{
    public static class Future
    {
        /// <summary>
        /// A future than never be completed.
        /// </summary>
        /// <returns></returns>
        public static Future<AsyncUnit> Infinite() => new Completer<AsyncUnit>().Future;

        /// <summary>
        /// A future whose value is available in the next event-loop iteration.
        /// </summary>
        public static Future<AsyncUnit> Value() => Value(AsyncUnit.Unit);

        /// <summary>
        /// A future whose value is available in the next event-loop iteration.
        /// </summary>
        public static Future<T> Value<T>(T value) => new Future<T>(Result.Value(value));

        /// <summary>
        /// A future that completes with an error in the next event-loop iteration.
        /// </summary>
        public static Future<AsyncUnit> Exception(Exception ex) =>
            new Future<AsyncUnit>(Result.Exception<AsyncUnit>(ex));

        /// <summary>
        /// A future that completes with an error in the next event-loop iteration.
        /// </summary>
        public static Future<T> Exception<T>(Exception ex) => new Future<T>(Result.Exception<T>(ex));

        /// <summary>
        /// Creates a future containing the result of immediately calling computation.
        /// </summary>
        public static Future<AsyncUnit> Sync(Action action)
        {
            try
            {
                action();
                return Value(AsyncUnit.Unit);
            }
            catch (Exception ex)
            {
                return Exception<AsyncUnit>(ex);
            }
        }

        /// <summary>
        /// Creates a future containing the result of immediately calling computation.
        /// </summary>
        public static Future<T> Sync<T>(Func<T> action)
        {
            try
            {
                var result = action();
                return Value(result);
            }
            catch (Exception ex)
            {
                return Exception<T>(ex);
            }
        }

        /// <summary>
        /// Creates a future containing the result of immediately calling computation.
        /// </summary>
        public static Future<T> Sync<T>(Func<Future<T>> action)
        {
            try
            {
                var result = action();
                return result;
            }
            catch (Exception ex)
            {
                return Exception<T>(ex);
            }
        }


        /// <summary>
        /// Returns the result of the first future in futures to complete.
        /// </summary>
        public static Future<T> Any<T>(IEnumerable<Future<T>> futures)
        {
            var resultFuture = new Future<T>();

            void Action(Result<T> val)
            {
                if (resultFuture.IsCompleted) return;

                resultFuture.CompleteResult(val);
            }

            foreach (var future in futures)
            {
                future.RegisterContinuation(Action);
            }

            return resultFuture;
        }

        /// <summary>
        /// Performs an operation repeatedly until it returns false.
        /// </summary>
        public static Future<AsyncUnit> DoWhile(Func<bool> f)
        {
            var resultFuture = new Future<AsyncUnit>();

            void Action()
            {
                try
                {
                    if (f())
                    {
                        Zone.Current.Invoke(Action);
                        return;
                    }

                    resultFuture.Complete(AsyncUnit.Unit);
                }
                catch (Exception ex)
                {
                    resultFuture.CompleteException(ex);
                }
            }

            Zone.Current.Invoke(Action);

            return resultFuture;
        }

        /// <summary>
        /// Perform an async operation for each element of the iterable, in turn.
        /// </summary>
        public static Future<AsyncUnit> ForEach<T>(IEnumerable<T> input, Func<T, Future<AsyncUnit>> f)
        {
            var resultFuture = new Future<AsyncUnit>();
            var enumerator = input.GetEnumerator();

            void Action()
            {
                if (enumerator.MoveNext())
                {
                    try
                    {
                        f(enumerator.Current).Then(Action).Catch(resultFuture.CompleteException);
                    }
                    catch (Exception ex)
                    {
                        resultFuture.CompleteException(ex);
                    }
                }
                else
                {
                    enumerator.Dispose();
                    resultFuture.Complete(AsyncUnit.Unit);
                }
            }

            Zone.Current.Invoke(Action);

            return resultFuture;
        }

        /// <summary>
        /// Wait for all the given futures to complete.
        /// </summary>
        public static Future<AsyncUnit> All(params Future<AsyncUnit>[] futures)
        {
            return All((IEnumerable<Future<AsyncUnit>>) futures);
        }

        /// <summary>
        /// Wait for all the given futures to complete.
        /// </summary>
        public static Future<AsyncUnit> All(IEnumerable<Future<AsyncUnit>> futures)
        {
            var resultFuture = new Future<AsyncUnit>();
            int remaining = 0;
            Exception exception = null;

            void Action(Result<AsyncUnit> val)
            {
                // ReSharper disable once AccessToModifiedClosure
                --remaining;

                if (val.IsException && exception == null)
                    exception = val.Exception;

                if (remaining == 0)
                {
                    if (exception == null)
                    {
                        resultFuture.Complete(AsyncUnit.Unit);
                    }
                    else
                    {
                        resultFuture.CompleteException(exception);
                    }
                }
            }

            foreach (var future in futures)
            {
                remaining++;
                future.RegisterContinuation(Action);
            }

            return resultFuture;
        }

        /// <summary>
        /// Wait for all the given futures to complete.
        /// </summary>
        public static Future<T[]> All<T>(params Future<T>[] futures)
        {
            return All((IEnumerable<Future<T>>) futures);
        }

        /// <summary>
        /// Wait for all the given futures to complete.
        /// </summary>
        public static Future<T[]> All<T>(IEnumerable<Future<T>> futures)
        {
            var resultFuture = new Future<T[]>();
            int remaining = 0;
            Exception exception = null;
            T[] results = null;

            void Action(Result<T> val, int index)
            {
                // ReSharper disable once AccessToModifiedClosure
                --remaining;

                if (val.IsException && exception == null)
                    exception = val.Exception;

                if (val.IsValue)
                {
                    // ReSharper disable once AccessToModifiedClosure
                    // ReSharper disable once PossibleNullReferenceException
                    results[index] = val.Value;
                }

                if (remaining == 0)
                {
                    if (exception == null)
                    {
                        // ReSharper disable once AccessToModifiedClosure
                        resultFuture.Complete(results);
                    }
                    else
                    {
                        resultFuture.CompleteException(exception);
                    }
                }
            }

            foreach (var future in futures)
            {
                int index = remaining++;
                future.RegisterContinuation(val => Action(val, index));
            }

            results = new T[remaining];

            return resultFuture;
        }

        /// <summary>
        /// Creates a future that completes after a delay.
        /// </summary>
        public static Future<AsyncUnit> Delayed(TimeSpan delay) => Delayed((float) delay.TotalSeconds);

        /// <summary>
        /// Creates a future that runs its computation after a delay.
        /// </summary>
        public static Future<AsyncUnit> Delayed(TimeSpan delay, Action computation)
            => Delayed((float) delay.TotalSeconds, computation);

        /// <summary>
        /// Creates a future that runs its computation after a delay.
        /// </summary>
        public static Future<AsyncUnit> Delayed(TimeSpan delay, Func<Future<AsyncUnit>> computation)
            => Delayed((float) delay.TotalSeconds, computation);

        /// <summary>
        /// Creates a future that runs its computation after a delay.
        /// </summary>
        public static Future<T> Delayed<T>(TimeSpan delay, Func<T> computation)
            => Delayed((float) delay.TotalSeconds, computation);

        /// <summary>
        /// Creates a future that runs its computation after a delay.
        /// </summary>
        public static Future<T> Delayed<T>(TimeSpan delay, Func<Future<T>> computation)
            => Delayed((float) delay.TotalSeconds, computation);

        /// <summary>
        /// Creates a future that completes after a delay.
        /// </summary>
        public static Future<AsyncUnit> Delayed(float delay)
        {
            var future = new Future<AsyncUnit>();
            Zone.Current.InvokeDelayed(delay, () => { future.Complete(AsyncUnit.Unit); });
            return future;
        }

        /// <summary>
        /// Creates a future that runs its computation after a delay.
        /// </summary>
        public static Future<AsyncUnit> Delayed(float delay, Action computation)
        {
            var future = new Future<AsyncUnit>();
            Zone.Current.InvokeDelayed(delay, () => { Sync(computation).TransitTo(future); });
            return future;
        }

        /// <summary>
        /// Creates a future that runs its computation after a delay.
        /// </summary>
        public static Future<AsyncUnit> Delayed(float delay, Func<Future<AsyncUnit>> computation)
        {
            var future = new Future<AsyncUnit>();
            Zone.Current.InvokeDelayed(delay, () => { Sync(computation).TransitTo(future); });
            return future;
        }

        /// <summary>
        /// Creates a future that runs its computation after a delay.
        /// </summary>
        public static Future<T> Delayed<T>(float delay, Func<T> computation)
        {
            var future = new Future<T>();
            Zone.Current.InvokeDelayed(delay, () => { Sync(computation).TransitTo(future); });
            return future;
        }

        /// <summary>
        /// Creates a future that runs its computation after a delay.
        /// </summary>
        public static Future<T> Delayed<T>(float delay, Func<Future<T>> computation)
        {
            var future = new Future<T>();
            Zone.Current.InvokeDelayed(delay, () => { Sync(computation).TransitTo(future); });
            return future;
        }

        /// <summary>
        /// Creates a future containing the result of calling computation asynchronously.
        /// </summary>
        public static Future<AsyncUnit> Run(Action computation)
        {
            var future = new Future<AsyncUnit>();
            Zone.Current.Invoke(() => { Sync(computation).TransitTo(future); });
            return future;
        }

        /// <summary>
        /// Creates a future containing the result of calling computation asynchronously.
        /// </summary>
        public static Future<AsyncUnit> Run(Func<Future<AsyncUnit>> computation)
        {
            var future = new Future<AsyncUnit>();
            Zone.Current.Invoke(() => { Sync(computation).TransitTo(future); });
            return future;
        }

        /// <summary>
        /// Creates a future containing the result of calling computation asynchronously.
        /// </summary>
        public static Future<T> Run<T>(Func<T> computation)
        {
            var future = new Future<T>();
            Zone.Current.Invoke(() => { Sync(computation).TransitTo(future); });
            return future;
        }

        /// <summary>
        /// Creates a future containing the result of calling computation asynchronously.
        /// </summary>
        public static Future<T> Run<T>(Func<Future<T>> computation)
        {
            var future = new Future<T>();
            Zone.Current.Invoke(() => { Sync(computation).TransitTo(future); });
            return future;
        }
    }
}