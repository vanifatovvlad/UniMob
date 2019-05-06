using System;

namespace UniMob.Async
{
    internal struct Result<T>
    {
        public ResultState State;
        public T Value;
        public Exception Exception;

        public bool IsException => State == ResultState.Exception;
        public bool IsValue => State == ResultState.Value;

        public override string ToString()
        {
            return $"[{State} {Value}{Exception}]";
        }
    }
}