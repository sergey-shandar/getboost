using builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace builderTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void VersionTestMethod()
        {
            Assert.AreEqual(new UnstableVersion(1, 56, 0, "rc1").ToString(), "1.56.0-rc1");
            Assert.AreEqual(new StableVersion(1, 55, 0, 16).ToString(), "1.55.0.16");
        }
    }
}
