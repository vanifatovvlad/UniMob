using System;
using NUnit.Framework;

namespace UniMob.Async.Tests.Futures
{
    public class FutureThenTests
    {
        [Test]
        public void Then_ByAction_RunsParallel() => TestZone.Run(tick =>
        {
            string strA = "1";
            string strB = "2";

            var future = Future.Value(12);
            var futureA = future.Then(() => { strA += "a"; });
            var futureB = future.Then(() => { strB += "b"; });

            Assert.AreEqual("1", strA);
            Assert.AreEqual("2", strB);
            Assert.IsFalse(futureA.IsCompleted);
            Assert.IsFalse(futureB.IsCompleted);

            tick(0);

            Assert.AreEqual("1a", strA);
            Assert.AreEqual("2b", strB);
            Assert.AreEqual(Result.Value(12), futureA.Value);
            Assert.AreEqual(Result.Value(12), futureB.Value);
        });

        [Test]
        public void Then_ByAction_RunsChain() => TestZone.Run(tick =>
        {
            string str = "a";

            var future = Future.Value(12)
                .Then(() => { str += "b"; })
                .Then(() => { str += "c"; })
                .Then(() => { str += "d"; });

            Assert.AreEqual("a", str);
            Assert.IsFalse(future.IsCompleted);

            tick(0);

            Assert.AreEqual("abcd", str);
            Assert.AreEqual(Result.Value(12), future.Value);
        });

        [Test]
        public void Then_ByAction() => TestZone.Run(tick =>
        {
            int value = 0;

            Future.Value(12).Then(() => ++value);
            Assert.AreEqual(0, value);

            tick(0);
            Assert.AreEqual(1, value);
        });

        [Test]
        public void Then_ByAction_WhenException() => TestZone.Run(ex => { }, tick =>
        {
            int value = 0;

            Future.Exception<int>(new Exception()).Then(() => ++value);
            Assert.AreEqual(0, value);

            tick(0);
            Assert.AreEqual(0, value);
        });

        [Test]
        public void Then_ByAction_WithArg() => TestZone.Run(tick =>
        {
            int value = 0;

            var future = Future.Value(12).Then(arg =>
            {
                Assert.AreEqual(12, arg);
                ++value;
            });
            Assert.AreEqual(0, value);

            tick(0);
            Assert.AreEqual(1, value);
            Assert.AreEqual(Result.Value(12), future.Value);
        });

        [Test]
        public void Then_ByFuncFuture() => TestZone.Run(tick =>
        {
            var future = Future.Value(12).Then(() =>
            {
                //
                return Future.Value("ab");
            });

            tick(0);

            Assert.AreEqual(Result.Value("ab"), future.Value);
        });

        [Test]
        public void Then_ByFuncValue_withArg() => TestZone.Run(tick =>
        {
            var future = Future.Value(12).Then(arg =>
            {
                //
                return "ab";
            });

            tick(0);

            Assert.AreEqual(Result.Value("ab"), future.Value);
        });
        
        [Test]
        public void Then_ByFuncFuture_withArg() => TestZone.Run(tick =>
        {
            var future = Future.Value(12).Then(arg =>
            {
                //
                return Future.Value("ab");
            });

            tick(0);

            Assert.AreEqual(Result.Value("ab"), future.Value);
        });
    }
}