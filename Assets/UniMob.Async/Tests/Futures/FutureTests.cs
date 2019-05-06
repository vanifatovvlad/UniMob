using System;
using NUnit.Framework;

namespace UniMob.Async.Tests.Futures
{
    public class FutureTests
    {
        [Test]
        public void Value() => TestZone.Run(_ =>
        {
            var future = Future.Value(12);
            Assert.AreEqual(Result.Value(12), future.Value);
        });

        [Test]
        public void Exception() => TestZone.Run(_ =>
        {
            var exception = new Exception("Test ex");
            var future = Future.Exception<AsyncUnit>(exception);
            Assert.AreEqual(Result.Exception<AsyncUnit>(exception), future.Value);
        });

        [Test]
        public void ToUnit() => TestZone.Run(tick =>
        {
            var future = Future.Value(12).ToUnit();            
            Assert.IsFalse(future.IsCompleted);

            tick(0);
            Assert.AreEqual(Result.Value(AsyncUnit.Unit), future.Value);
        });
    }
}