using System;
using NUnit.Framework;

namespace UniMob.Async.Tests.Futures
{
    public class FutureTimeoutTests
    {
        [Test]
        public void Timeout_ByAction_NotReached() => TestZone.Run(tick =>
        {
            int timeout = 0;
            //

            var future = Future.Value(12).Timeout(TimeSpan.FromSeconds(2), () =>
            {
                //
                ++timeout;
            });

            Assert.AreEqual(0, timeout);

            tick(2.1f);
            Assert.AreEqual(0, timeout);
            Assert.AreEqual(Result.Value(AsyncUnit.Unit), future.Value);
        });

        [Test]
        public void Timeout_ByAction_Reached() => TestZone.Run(tick =>
        {
            int timeout = 0;
            //

            var future = Future.Infinite().Timeout(TimeSpan.FromSeconds(2), () =>
            {
                //
                ++timeout;
            });

            Assert.AreEqual(0, timeout);

            tick(1.9f);
            Assert.AreEqual(0, timeout);

            tick(2.1f);
            Assert.AreEqual(1, timeout);
            Assert.AreEqual(Result.Value(AsyncUnit.Unit), future.Value);
        });

        [Test]
        public void Timeout_ByFuncValue_NotReached() => TestZone.Run(tick =>
        {
            int timeout = 0;
            //

            var future = Future.Value(12).Timeout(TimeSpan.FromSeconds(2), () =>
            {
                //
                ++timeout;
                return 34;
            });

            Assert.AreEqual(0, timeout);

            tick(2.1f);
            Assert.AreEqual(0, timeout);
            Assert.AreEqual(Result.Value(12), future.Value);
        });

        [Test]
        public void Timeout_ByFuncValue_Reached() => TestZone.Run(tick =>
        {
            int timeout = 0;
            //

            var future = new Completer<int>().Future.Timeout(TimeSpan.FromSeconds(2), () =>
            {
                //
                ++timeout;
                return 34;
            });

            Assert.AreEqual(0, timeout);

            tick(1.9f);
            Assert.AreEqual(0, timeout);

            tick(2.1f);
            Assert.AreEqual(1, timeout);
            Assert.AreEqual(Result.Value(34), future.Value);
        });

        [Test]
        public void Timeout_ByFuncFuture_NotReached() => TestZone.Run(tick =>
        {
            int timeout = 0;
            //

            var future = Future.Value(12).Timeout(TimeSpan.FromSeconds(2), () =>
            {
                //
                ++timeout;
                return Future.Value(34);
            });

            Assert.AreEqual(0, timeout);

            tick(2.1f);
            Assert.AreEqual(0, timeout);
            Assert.AreEqual(Result.Value(12), future.Value);
        });

        [Test]
        public void Timeout_ByFuncFuture_Reached() => TestZone.Run(tick =>
        {
            int timeout = 0;
            //

            var future = new Completer<int>().Future.Timeout(TimeSpan.FromSeconds(2), () =>
            {
                //
                ++timeout;
                return Future.Value(34);
            });

            Assert.AreEqual(0, timeout);

            tick(1.9f);
            Assert.AreEqual(0, timeout);

            tick(2.1f);
            Assert.AreEqual(1, timeout);
            Assert.AreEqual(Result.Value(34), future.Value);
        });
    }
}