using System;
using System.Runtime.CompilerServices;

namespace UniMob.Async
{
    internal static class Result
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Result<T> Value<T>(T value)
        {
            return new Result<T>
            {
                State = ResultState.Value,
                Value = value,
                Exception = null,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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