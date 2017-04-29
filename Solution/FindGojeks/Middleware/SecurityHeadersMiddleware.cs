using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using FindGojeks.Middleware.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace FindGojeks.Middleware
{
    /// <summary>
    /// An ASP.NET middleware for adding security headers.
    /// </summary>
    public class SecurityHeadersMiddleware
    {
        private RequestDelegate _next;
        private readonly SecurityHeadersPolicy _policy;

        /// <summary>
        /// Instantiates a new <see cref="SecurityHeadersMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="policy">An instance of the <see cref="SecurityHeadersPolicy"/> which can be applied.</param>
        public SecurityHeadersMiddleware(RequestDelegate next, SecurityHeadersPolicy policy)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (next == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            _next = next;
            _policy = policy;
        }

        public async Task Invoke(HttpContext context)
        {            	
            var watch = new Stopwatch();
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.Response;

            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            watch = new Stopwatch();
            context.Response.OnStarting(() =>
            {
                watch.Stop();

                context
                    .Response
                    .Headers
                    .Add("GOJEK-API-PROCESSING-ELAPSEDTIME",new[] { watch.ElapsedMilliseconds.ToString() });

                return Task.CompletedTask;
            });

            watch.Start();

            var headers = response.Headers;

            foreach (var headerValuePair in _policy.SetHeaders)
            {
                headers[headerValuePair.Key] = headerValuePair.Value;                
            }
            
            foreach (var header in _policy.RemoveHeaders)
            {
                headers.Remove(header);
            }                
		     		
            await _next(context); 
            
        }

    }
}
