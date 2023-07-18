using SoftUniWebServer.Server.Common;
using SoftUniWebServer.Server.HTTP;
using SoftUniWebServer.Server.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftUniWebServer.Server
{
    public class RoutingTable : IRoutingTable
    {
        private readonly Dictionary<Method, Dictionary<string, Func<Request, Response>>> _routes;

        public RoutingTable()
            => _routes = new()
            {
                [Method.Get] = new(StringComparer.InvariantCultureIgnoreCase),
                [Method.Post] = new(StringComparer.InvariantCultureIgnoreCase),
                [Method.Put] = new(StringComparer.InvariantCultureIgnoreCase),
                [Method.Delete] = new(StringComparer.InvariantCultureIgnoreCase),
            };


        public IRoutingTable Map(Method method, string path, Func<Request, Response> responseFunction)
        {
            Guard.AgainstNull(path, nameof(path));
            Guard.AgainstNull(responseFunction, nameof(responseFunction));

            _routes[method][path] = responseFunction;

            return this;
        }

        public IRoutingTable MapGet(string path, Func<Request, Response> responseFunction) => Map(Method.Get, path, responseFunction);

        public IRoutingTable MapPost(string path, Func<Request, Response> responseFunction) => Map(Method.Post, path, responseFunction);

        public Response MatchRequest(Request request)
        {
            var requestMethod = request.Method;
            var requestUrl = request.Url;

            if (!_routes.ContainsKey(requestMethod) || !_routes[requestMethod].ContainsKey(requestUrl))
            {
                return new NotFoundResponse();
            }

            var responseFunction = _routes[requestMethod][requestUrl];

            return responseFunction(request);
        }
    }
}
