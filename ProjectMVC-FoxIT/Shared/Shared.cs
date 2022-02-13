using Microsoft.AspNetCore.Http;
using Postal;

namespace WorkOrders.Shared
{
    public class Shared
    {
        public static object SharedRenamed { get; internal set; }

        public static RequestPath PostalRequest(HttpRequest Request)
        {
            var requestPath = new Postal.RequestPath();
            requestPath.PathBase = Request.PathBase.ToString();
            requestPath.Host = Request.Host.ToString();
            requestPath.IsHttps = Request.IsHttps;
            requestPath.Scheme = Request.Scheme;
            requestPath.Method = Request.Method;

            return requestPath;
        }
    }
}
