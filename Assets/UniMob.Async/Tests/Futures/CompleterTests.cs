using System;
using NUnit.Framework;
using UnityEngine;

namespace UniMob.Async.Tests.Futures
{
    public class CompleterTests
    {
        [Test]
        public void CompleteValue() => TestZone.Run(tick =>
        {
            var completer = new Completer<int>();

            Assert.IsFalse(completer.IsCompleted);

            tick(0);
            Assert.IsFalse(completer.IsCompleted);

            completer.Complete(12);
            Assert.Catch<InvalidOperationException>(() => completer.Complete(34));
            completer.TryComplete(56);
            
            tick(0);
            Assert.AreEqual(Result.Value(12), completer.Future.Value);
        });

        [Test]
        public void CompleteException() => TestZone.Run(ex => { }, tick =>
        {
            var completer = new Completer<int>();

            Assert.IsFalse(completer.IsCompleted);

            tick(0);
            Assert.IsFalse(completer.IsCompleted);

            var exception = new Exception("Test ex");
            completer.CompleteException(exception);
            Assert.Catch<InvalidOperationException>(() => completer.CompleteException(new UnityException("ignored")));
            completer.TryCompleteException(new UnityException("Ignored 2"));

            tick(0);
            Assert.AreEqual(Result.Exception<int>(exception), completer.Future.Value);
        });
    }
}