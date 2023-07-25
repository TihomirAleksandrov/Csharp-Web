using SoftUniWebServer.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftUniWebServer.Server.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpMethodAttribute : Attribute
    {
        public Method HttpMethod { get; }

        protected HttpMethodAttribute(Method httpMethod)
            => HttpMethod = httpMethod;
    }
}
