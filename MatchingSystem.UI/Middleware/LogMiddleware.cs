using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MatchingSystem.UI.Middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string connectionString;

        public LogMiddleware(RequestDelegate next, string connectionString)
        {
            this.next = next;
            this.connectionString = connectionString;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ILoggerRepository repository = new LogRepository(connectionString);
            if (context.Request.Path.Value != null && context.Request.Path.Value.Contains("/api"))
            {
                try
                {
                    var requestParams = new StringBuilder(string.Empty);

                    if (context.Request.HasFormContentType == true)
                    {
                        var form = await context.Request.ReadFormAsync();
                        WriteRequestForm(requestParams, form);
                    }

                    
                    await WriteRequestBody(requestParams, context.Request.Body);
                    WriteRequestQuery(requestParams, context.Request.Query);

                    repository.LogRequest(requestParams.ToString(), context.Request.Path.ToString());
                }
                catch (Exception ex)
                {
                    await next.Invoke(context);
                }
            }

            await next.Invoke(context);
        }

        private void WriteRequestForm(StringBuilder writer, IFormCollection form)
        {
            writer.AppendLine("[FORM]");
            foreach (var data in form)
            {
                writer.AppendLine($@"{data.Key} : {{");
                foreach (var value in data.Value)
                {
                    writer.AppendLine($"\t{value}");
                }

                writer.AppendLine("}");
            }

            writer.AppendLine("[ENDFORM]");
        }

        private async Task WriteRequestBody(StringBuilder writer, Stream bodyStream)
        {
            var reader = new StreamReader(bodyStream);
            writer.AppendLine("[BODY]");
            writer.AppendLine("\t" + await reader.ReadToEndAsync());
            writer.AppendLine("[ENDBODY]");
        }

        private void WriteRequestQuery(StringBuilder writer, IQueryCollection query)
        {
            writer.AppendLine("[QUERY]");

            foreach (var data in query)
            {
                writer.AppendLine($@"{data.Key} : {{");
                foreach (var value in data.Value)
                {
                    writer.AppendLine($"\t{value}");
                }

                writer.AppendLine("}");
            }

            writer.AppendLine("[ENDQUERY]");
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
