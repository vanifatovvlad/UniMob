using System;
using NUnit.Framework;
using UnityEngine;

namespace UniMob.Async.Tests
{
    public class FutureCatchTests
    {
        [Test]
        public void UnhandledException_HandledByZone()
        {
            var exception = new Exception("Test ex");

            TestZone.Run(ex => Assert.AreEqual(exception, ex), tick =>
            {
                var future = Future.Value().Then(() => throw exception);
                tick(0);
                Assert.AreEqual(Result.Exception<AsyncUnit>(exception), future.Value);
            });
        }

        [Test]
        public void Catch_ByAction() => TestZone.Run(tick =>
        {
            var exception = new Exception("Test ex");
            int value = 0;

            var future = Future.Value(12)
                .Then(() => throw exception)
                .Catch(ex =>
                {
                    Assert.AreEqual(exception, ex);
                    ++value;
                });

            Assert.AreEqual(0, value);

            tick(0);
            Assert.AreEqual(Result.Value(AsyncUnit.Unit), future.Value);
            Assert.AreEqual(1, value);
        });


        [Test]
        public void CatchTyped_ByAction_IfTypeWrong_Unhandled()
        {
            var exception = new Exception("Test ex");
            int valueErr = 0;

            TestZone.Run(ex =>
            {
                Assert.AreEqual(exception, ex);
                ++valueErr;
            }, tick =>
            {
                var future = Future.Value(12)
                    .Then(() => throw exception)
                    .Catch<UnityException>(ex => Assert.Fail(ex.Message));

                tick(0);
                Assert.AreEqual(Result.Exception<AsyncUnit>(exception), future.Value);
            });
            
            Assert.AreEqual(1, valueErr);
        }

        [Test]
        public void CatchTyped_ByAction() => TestZone.Run(tick =>
        {
            var exception = new UnityException("Test ex");
            int value = 0;

            var future = Future.Value(12)
                .Then(() => throw exception)
                .Catch<UnityException>(ex =>
                {
                    Assert.AreEqual(exception, ex);
                    ++value;
                });

            Assert.AreEqual(0, value);

            tick(0);
            Assert.AreEqual(Result.Value(AsyncUnit.Unit), future.Value);
            Assert.AreEqual(1, value);
        });

        [Test]
        public void Catch_ByFunc_ReturnsValue() => TestZone.Run(tick =>
        {
            var exception = new Exception("Test ex");

            var future = Future.Value(12)
                .Then(() => throw exception)
                .Catch(ex => 34);

            tick(0);
            Assert.AreEqual(Result.Value(34), future.Value);
        });

        [Test]
        public void Catch_ByFunc_ReturnsFuture() => TestZone.Run(tick =>
        {
            var exception = new Exception("Test ex");

            var future = Future.Value(12)
                .Then(() => throw exception)
                .Catch(ex => Future.Value(34));

            tick(0);
            Assert.AreEqual(Result.Value(34), future.Value);
        });

        [Test]
        public void CatchTyped_ByFunc_IfTypeWrong_Unhandled()
        {
            var exception = new Exception("Test ex");
            int errorCount = 0;

            TestZone.Run(ex =>
            {
                Assert.AreEqual(exception, ex);
                errorCount++;
                //
            }, tick =>
            {
                var future = Future.Value(12)
                    .Then(() => throw exception)
                    .Catch<UnityException>(ex =>
                    {
                        Assert.Fail(ex.Message);
                        return 34;
                    });

                tick(0);
                Assert.AreEqual(Result.Exception<int>(exception), future.Value);
            });

            Assert.AreEqual(1, errorCount);
        }

        [Test]
        public void CatchTyped_ByFunc_ReturnsValue() => TestZone.Run(tick =>
        {
            var exception = new UnityException("Test ex");

            var future = Future.Value(12)
                .Then(() => throw exception)
                .Catch<UnityException>(ex => 34);

            tick(0);
            Assert.AreEqual(Result.Value(34), future.Value);
        });

        [Test]
        public void CatchTyped_ByFunc_ReturnsFuture() => TestZone.Run(tick =>
        {
            var exception = new UnityException("Test ex");

            var future = Future.Value(12)
                .Then(() => throw exception)
                .Catch<UnityException>(ex => Future.Value(34));

            tick(0);
            Assert.AreEqual(Result.Value(34), future.Value);
        });
    }
}