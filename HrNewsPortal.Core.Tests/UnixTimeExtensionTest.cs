using System;
using HrNewsPortal.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HrNewsPortal.Core.Tests
{
    [TestClass]
    public class UnixTimeExtensionTest
    {
        [TestMethod]
        public void ConvertUnixTimeTest()
        {
            var unixEpochTime = 1584207289;
            var expectedDateTime = new DateTime?(new DateTime(2020, 03, 14, 17, 34, 49));
            var actualDateTime = unixEpochTime.GetDateTime();

            Assert.AreEqual(expectedDateTime.GetValueOrDefault().Year, actualDateTime.GetValueOrDefault().Year);
            Assert.AreEqual(expectedDateTime.GetValueOrDefault().Month, actualDateTime.GetValueOrDefault().Month);
            Assert.AreEqual(expectedDateTime.GetValueOrDefault().Day, actualDateTime.GetValueOrDefault().Day);
            Assert.AreEqual(expectedDateTime.GetValueOrDefault().Hour, actualDateTime.GetValueOrDefault().Hour);
            Assert.AreEqual(expectedDateTime.GetValueOrDefault().Minute, actualDateTime.GetValueOrDefault().Minute);
            Assert.AreEqual(expectedDateTime.GetValueOrDefault().Second, actualDateTime.GetValueOrDefault().Second);
        }
    }
}
