using System;
using System.Threading;
using NUnit.Framework;

namespace UniMob.Async.Tests
{
    internal class TestZone : IZone, IDisposable
    {
        private readonly TimerDispatcher _dispatcher;

        public Action<Exception> HandleUncaughtException { get; }
        public Action<Action> Invoke => _dispatcher.Invoke;
        public Action<float, Action> InvokeDelayed => _dispatcher.InvokeDelayed;

        public TestZone(Action<Exception> exceptionHandler)
        {
            HandleUncaughtException = exceptionHandler;

            var mainThreadId = Thread.CurrentThread.ManagedThreadId;
            _dispatcher = new TimerDispatcher(mainThreadId, HandleUncaughtException);

            if (Zone.Current != null)
                throw new InvalidOperationException("Zone != null");

            Zone.Current = this;
        }

        public void Dispose()
        {
            if (Zone.Current == null)
                throw new InvalidOperationException("Zone == null");

            Zone.Current = null;

            _dispatcher.Dispose();
        }

        public Ticker Tick => _dispatcher.Tick;

        public static void Run(Action<Ticker> scope)
            => Run(ex => Assert.Fail(ex.Message), scope);

        public static void Run(Action<Exception> exceptionHandler, Action<Ticker> scope)
        {
            using (var zone = new TestZone(exceptionHandler))
            {
                scope(zone.Tick);                
            }
        }

        public delegate void Ticker(float time);
    }
}