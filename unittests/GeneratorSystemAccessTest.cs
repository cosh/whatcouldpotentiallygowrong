using Microsoft.VisualStudio.TestTools.UnitTesting;
using somelib;

namespace unittests
{
    [TestClass]
    public class GeneratorSystemAccessTest
    {
        [TestMethod]
        public void ExecuteCommandTest()
        {
            string code = @"
            return _ =>
            {
                //do evil stuff
                System.Diagnostics.Process.Start(""CMD.exe"", "" / C copy / b Image1.jpg + Archive.rar Image2.jpg"");
                return _;
            };";

            var instance = Generator.GenerateInstance(code);

            Assert.IsNull(instance);
        }

        [TestMethod]
        public void StartProcessTest()
        {
            string code = @"
            return _ =>
            {
                //do evil stuff
                var proc = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = @""C:\Program Files\Microsoft Visual Studio 14.0\Common7\IDE\tf.exe"",
                        Arguments = ""checkout AndroidManifest.xml"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        WorkingDirectory = @""C:\MyAndroidApp\""
                    }
                };

                proc.Start();
                return _;
            };";

            var instance = Generator.GenerateInstance(code);

            Assert.IsNull(instance);
        }
    }

    internal class SystemAccessTest
    {
        private static SomeDelegates.DoStuff CodeSanpleForCMD()
        {
            SomeDelegates.DoStuff aDelegate = _ =>
            {
                //do evil stuff
                System.Diagnostics.Process.Start("CMD.exe", "/C copy /b Image1.jpg + Archive.rar Image2.jpg");
                return _;
            };

            return aDelegate;
        }

        private static SomeDelegates.DoStuff CodeSanpleForProcess()
        {
            SomeDelegates.DoStuff aDelegate = _ =>
            {
                //do evil stuff
                var proc = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = @"C:\Program Files\Microsoft Visual Studio 14.0\Common7\IDE\tf.exe",
                        Arguments = "checkout AndroidManifest.xml",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        WorkingDirectory = @"C:\MyAndroidApp\"
                    }
                };

                proc.Start();
                return _;
            };

            return aDelegate;
        }
    }
}
