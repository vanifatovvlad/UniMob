using System;
using NUnit.Framework;

namespace UniMob.Async.Tests
{
    public class ResultTests
    {        
        [Test]
        public void Create_Value()
        {
            var asyncValue = Result.Value<int>(123);
            Assert.AreEqual(ResultState.Value, asyncValue.State);
            Assert.AreEqual(123, asyncValue.Value);
            Assert.AreEqual(null, asyncValue.Exception);
        }
        
        [Test]
        public void Create_Exception()
        {
            var exception = new Exception();
            var asyncValue = Result.Exception<int>(exception);
            Assert.AreEqual(ResultState.Exception, asyncValue.State);
            Assert.AreEqual(default(int), asyncValue.Value);
            Assert.AreEqual(exception, asyncValue.Exception);
        }
    }
}
