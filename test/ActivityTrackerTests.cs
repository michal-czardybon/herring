using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Herring;

namespace test
{
    [TestClass]
    public class ActivityTrackerTests
    {
        [TestMethod]
        public void Test_AreTitlesNearlyEqual()
        {
            string common;
            Assert.AreEqual(true, ActivityTracker.AreTitlesNearlyEqual("Inbox (58) out", "Inbox (58) out", 3, out common));
            Assert.AreEqual("Inbox (58) out", common);
            Assert.AreEqual(true, ActivityTracker.AreTitlesNearlyEqual("aaaaa", "aaa", 3, out common));
            Assert.AreEqual("aaa**", common);
            Assert.AreEqual(true, ActivityTracker.AreTitlesNearlyEqual("Inbox (58) out", "Inbox (89) out", 3, out common));
            Assert.AreEqual("Inbox (**) out", common);
            Assert.AreEqual(true, ActivityTracker.AreTitlesNearlyEqual("Inbox (58) out", "Inbox (893) out", 3, out common));
            Assert.AreEqual("Inbox (***) out", common);
            Assert.AreEqual(true, ActivityTracker.AreTitlesNearlyEqual("Inbox (583) out", "Inbox (89) out", 3, out common));
            Assert.AreEqual("Inbox (***) out", common);
            Assert.AreEqual(false, ActivityTracker.AreTitlesNearlyEqual("Inbox (5832) out", "Inbox (89) out", 3, out common));
            Assert.AreEqual("", common);
            Assert.AreEqual(true, ActivityTracker.AreTitlesNearlyEqual("Inbox (*) out", "Inbox (89) out", 3, out common));
            Assert.AreEqual("Inbox (**) out", common);
        }

        [TestMethod]
        public void Test_AreTitlesNearlyEqual_Email()
        {
            string common;
            string a = "Inbox - jankowalski@gmail-fake.com (2* messages) - Postbox";
            string b = "Inbox - jankowalski@gmail-fake.com (** messages) - Postbox";
            Assert.AreEqual(true, ActivityTracker.AreTitlesNearlyEqual(a, b, 3, out common));
            Assert.AreEqual(b, common);
        }
    }
}
