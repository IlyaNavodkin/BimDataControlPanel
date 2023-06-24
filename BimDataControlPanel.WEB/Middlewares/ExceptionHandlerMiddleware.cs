using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using BimDataControlPanel.DAL.Exeptions;
using Microsoft.AspNetCore.Mvc;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BimDataControlPanel.WEB.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly IHostEnvironment _environment;
    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger, 
        IHostEnvironment hostEnvironment)
    {
        _next = next;
        _logger = logger;
        _environment = hostEnvironment;
    }

    public async Task Invoke(HttpContext context)  
    {  
        try  
        {  
            await _next.Invoke(context);  
        }  
        catch (Exception ex)  
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                await GenerateJsonResponse(context, ex);
            }
            else throw;
        }  
    }  
    
    private async Task GenerateJsonResponse(HttpContext context, Exception propertyException)
    {
        ProblemDetails problemDetails = null;
        
        if (propertyException is ValidationException validationException)
        {
            var detailList = validationException.Errors
                .Select(error => error.ErrorMessage)
                .ToList();
            
            var detail = String.Join("\n",detailList);
            
            problemDetails = new ProblemDetails
            {
                Title = nameof(validationException),
                Status = (int)HttpStatusCode.BadRequest,
                Detail = detail
            };
        }
        else
        {
            problemDetails = new ProblemDetails
            {
                Title = nameof(propertyException),
                Status = (int)HttpStatusCode.BadRequest,
                Detail = propertyException.Message
            };
        }

        await CreateResponse(context, problemDetails);
    }

    private static async Task CreateResponse(HttpContext context, ProblemDetails problemDetails)
    {
        context.Response.Clear();
        context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        var json = JsonSerializer.Serialize(problemDetails, options);

        await context.Response.WriteAsync(json);
    }
}