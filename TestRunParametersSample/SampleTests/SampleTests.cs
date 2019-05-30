using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SampleTests
{
    [TestClass]
    public class SampleTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void SampleTest()
        {
            const string ParameterName = "password";
            const string ExpectedValue = "secret";
            Assert.AreEqual(ExpectedValue, TestContext.Properties[ParameterName]);
        }
    }
}