using SoftUniWebServer.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftUniWebServer.Server.Attributes
{
    public class HttpPostAttribute : HttpMethodAttribute
    {
        public HttpPostAttribute() : base(Method.Post)
        {
        }
    }
}
