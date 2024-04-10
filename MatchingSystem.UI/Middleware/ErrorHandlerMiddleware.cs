using System;
using System.Net;
using System.Threading.Tasks;
using MatchingSystem.Service.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MatchingSystem.UI.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ErrorHandlerMiddleware> logger;
    
    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (InputDataException ex)
        {
            logger.LogError("Error: {}\n Source:{}",ex.Message,ex.StackTrace);
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            
            var result = JsonSerializer.Serialize(ex.Message);
            await response.WriteAsync(result);
        }
        catch (Exception error)
        {
            logger.LogError("Error: {}\n Source:{}",error.Message,error.StackTrace);
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            
            var result = JsonSerializer.Serialize(error.Message);
           await response.WriteAsync(result);
        }
    }
}