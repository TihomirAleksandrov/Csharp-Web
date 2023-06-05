using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftUniWebServer.Server.HTTP
{
    public class HeaderCollection
    {
        private readonly Dictionary<string, Header> _headers;

        public HeaderCollection()
            => _headers = new Dictionary<string, Header>();

        public int Count => _headers.Count;

        public void Add(string name, string value)
        {
            var header = new Header(name, value);

            _headers.Add(name, header);
        }
    }
}
