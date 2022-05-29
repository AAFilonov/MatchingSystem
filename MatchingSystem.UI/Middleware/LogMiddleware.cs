using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MatchingSystem.UI.Middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<LogMiddleware> logger;
     

        public LogMiddleware(RequestDelegate next, string connectionString, ILogger<LogMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            if (context.Request.Path.Value != null && context.Request.Path.Value.Contains("/api"))
            {
                try
                {
                    var requestParams = new StringBuilder(string.Empty);

                    if (context.Request.HasFormContentType)
                    {
                        var form = await context.Request.ReadFormAsync();
                        WriteRequestForm(requestParams, form);
                    }
                    
                    await WriteRequestBody(requestParams, context.Request.Body);
                    WriteRequestQuery(requestParams, context.Request.Query);  
              
                    logger.LogInformation("INCOMING REQUEST: {}  on path:{}",requestParams,context.Request.Path.ToString());
                }
                catch (Exception e)
                {
                    logger.LogError("Exception during logging: {}" ,e.Message);
                    await next.Invoke(context);
                }
                finally
                {
                    context.Request.Body.Position = 0;
                }
            }

            await next.Invoke(context);
        }

        private void WriteRequestForm(StringBuilder writer, IFormCollection form)
        {
            writer.Append( JsonConvert.SerializeObject( form));
               return;
            foreach (var data in form)
            {
                writer.Append($@"{data.Key}: {{");
                foreach (var value in data.Value)
                {
                    writer.Append($"{value}");
                }

                writer.Append("}");
            }
        }

        private async Task WriteRequestBody(StringBuilder writer, Stream bodyStream)
        {
            var reader = new StreamReader(bodyStream);
            writer.Append( await reader.ReadToEndAsync());
        }

        private void WriteRequestQuery(StringBuilder writer, IQueryCollection query)
        {
            
            foreach (var data in query)
            {
                writer.Append("[");
                //writer.AppendLine( JsonConvert.SerializeObject( data));
                
                writer.Append($@"{data.Key}:");
                if (data.Value.Count > 1) 
                    writer.Append("{");
                foreach (var value in data.Value)
                {
                    writer.Append(JsonConvert.SerializeObject( value));
                }
                if (data.Value.Count > 1) 
                    writer.Append("}");
                writer.Append("]");
            }
            
        }
    }

    public static class LogMiddlewareExtention
    {
        public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder builder, string connectionString)
        {
            return builder.UseMiddleware<LogMiddleware>(connectionString);
        }
    }
}
