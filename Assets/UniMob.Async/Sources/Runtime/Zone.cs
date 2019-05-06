using System;

namespace UniMob.Async
{
    public abstract class Zone
    {
        public static IZone Current { get; set; }
    }

    public interface IZone
    {
        Action<Exception> HandleUncaughtException { get; }
        Action<Action> Invoke { get; }
        Action<float, Action> InvokeDelayed { get; }
    }
}