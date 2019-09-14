using System;

namespace UniMob.Async
{
    internal static class Result
    {
        public static Result<T> Value<T>(T value)
        {
            return new Result<T>
            {
                State = ResultState.Value,
                Value = value,
                Exception = null,
            };
        }

        public static Result<T> Exception<T>(Exception exception)
        {
            return new Result<T>
            {
                State = ResultState.Exception,
                Value = default,
                Exception = exception,
            };
        }
    }
}