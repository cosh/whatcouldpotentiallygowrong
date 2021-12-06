using Microsoft.VisualStudio.TestTools.UnitTesting;
using somelib;

namespace unittests
{
    [TestClass]
    public class GeneratorTest
    {
        [TestMethod]
        public void MostComplicatedWayOfDoublingANumber()
        {
            var instance = Generator.GenerateInstance("return _ => _ * 2;");

            var aDelegate = instance.DoOneThing();

            int radius = 10;

            var result = aDelegate(radius);

            Assert.AreEqual(20, result);
        }
    }
}
