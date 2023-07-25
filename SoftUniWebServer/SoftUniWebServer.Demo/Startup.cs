using SoftUniWebServer.Demo.Controllers;
using SoftUniWebServer.Server;
using SoftUniWebServer.Server.Controllers;
using SoftUniWebServer.Server.HTTP;
using SoftUniWebServer.Server.Responses;
using System.Text;
using System.Web;

namespace SoftUniWebServer.Demo
{
    internal class Startup
    {
        public static async Task Main()
            => await new HttpServer(routes => routes
            .MapControllers())
            .Start();
    }
}