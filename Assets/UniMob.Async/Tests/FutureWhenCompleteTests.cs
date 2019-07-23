using System;
using NUnit.Framework;

namespace UniMob.Async.Tests
{
    public class FutureWhenCompleteTests
    {
        [Test]
        public void WhenComplete_ByAction() => TestZone.Run(tick =>
        {
            int value = 0;

            var future = Future.Value(12).WhenComplete(() =>
            {
                //
                ++value;
            });

            tick(0);

            Assert.AreEqual(1, value);
            Assert.AreEqual(Result.Value(12), future.Value);
        });

        [Test]
        public void WhenComplete_ByAction_Exception() => TestZone.Run(ex => { }, tick =>
        {
            var exception = new Exception("Test ex");
            int value = 0;

            var future = Future.Exception<int>(exception).WhenComplete(() =>
            {
                //
                ++value;
            });

            tick(0);

            Assert.AreEqual(1, value);
            Assert.AreEqual(Result.Exception<int>(exception), future.Value);
        });

        [Test]
        public void WhenComplete_ByFunc() => TestZone.Run(tick =>
        {
            int value = 0;

            var future = Future.Value(12).WhenComplete(() =>
            {
                //
                ++value;
                
                return Future.Value();
            });

            tick(0);

            Assert.AreEqual(1, value);
            Assert.AreEqual(Result.Value(12), future.Value);
        });

        [Test]
        public void WhenComplete_ByFunc_Exception() => TestZone.Run(ex => { }, tick =>
        {
            var exception = new Exception("Test ex");
            int value = 0;

            var future = Future.Exception<int>(exception).WhenComplete(() =>
            {
                //
                ++value;

                return Future.Value();
            });

            tick(0);

            Assert.AreEqual(1, value);
            Assert.AreEqual(Result.Exception<int>(exception), future.Value);
        });
    }
}