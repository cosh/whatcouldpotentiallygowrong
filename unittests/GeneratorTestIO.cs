using Microsoft.VisualStudio.TestTools.UnitTesting;
using somelib;

namespace unittests
{
    [TestClass]
    public class GeneratorTestIO
    {
        [TestMethod]
        public void PathTest()
        {
            string code = @"
            return _ =>
            {
                //do evil stuff
                System.IO.Directory.GetFiles(""."");
                return _;
            };";

            var instance = Generator.GenerateInstance(code);

            Assert.IsNull(instance);
        }

        [TestMethod]
        public void ListenOnSocketTest()
        {
            string code = @"
            return _ =>
            {
                //do evil stuff
                System.Net.IPHostEntry ipHostInfo = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                System.Net.IPAddress ipAddress = ipHostInfo.AddressList[0];
                System.Net.IPEndPoint localEndPoint = new System.Net.IPEndPoint(ipAddress, 11000);

                System.Net.Sockets.Socket listener = new System.Net.Sockets.Socket(ipAddress.AddressFamily, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                listener.Listen(100);
                return _;
            };";

            var instance = Generator.GenerateInstance(code);

            Assert.IsNull(instance);
        }
    }

    internal class IOTest
    {
        private static SomeDelegates.DoStuff CodeSanpleForIO()
        {
            SomeDelegates.DoStuff aDelegate = _ =>
            {
                //do evil stuff
                System.IO.Directory.GetFiles(".");
                return _;
            };

            return aDelegate;
        }

        private static SomeDelegates.DoStuff CodeSanpleForSocket()
        {
            SomeDelegates.DoStuff aDelegate = _ =>
            {
                //do evil stuff
                System.Net.IPHostEntry ipHostInfo = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                System.Net.IPAddress ipAddress = ipHostInfo.AddressList[0];
                System.Net.IPEndPoint localEndPoint = new System.Net.IPEndPoint(ipAddress, 11000);

                System.Net.Sockets.Socket listener = new System.Net.Sockets.Socket(ipAddress.AddressFamily, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                listener.Listen(100);

                return _;
            };

            return aDelegate;
        }
    }
}
