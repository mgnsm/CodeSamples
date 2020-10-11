using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Containerized.Microservice.UnitTests
{
    [TestClass]
    public class SampleTests
    {
        [TestMethod]
        public void SampleTest()
        {
            //check that the returned string contains the name of the operating system
            string operatingSystem = HtmlRenderer.GetOperatingSystem();
            string html = HtmlRenderer.GetHtml();
            Assert.IsFalse(string.IsNullOrEmpty(html));
            Assert.IsTrue(html.Contains(operatingSystem));
        }
    }
}
