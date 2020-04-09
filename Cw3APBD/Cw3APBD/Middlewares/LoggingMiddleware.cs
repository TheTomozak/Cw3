using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Cw3APBD.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            if (httpContext.Request != null)
            {
                string path = httpContext.Request.Path;
                string method = httpContext.Request.Method;
                string queryString = httpContext.Request.QueryString.ToString();
                string bodyStr = "";

                using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                    httpContext.Request.Body.Position = 0;
                }

                using (var streamWriter = File.AppendText("Logs/requestsLog.txt"))
                {
                    string logInfoString = "Methods HTTP: " + method +
                                           "\nThe path to which the request was sent: " + path +
                                           "\nInformation from Query string: " + queryString +
                                           "\nHTTP body: " + bodyStr + "\n\n";
                    streamWriter.Write(logInfoString);
                }
            }

            if (_next != null)
            {
                await _next(httpContext);
            }
        }
    }
}