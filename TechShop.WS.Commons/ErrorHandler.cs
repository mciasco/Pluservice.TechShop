using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace TechShop.WS.Commons
{
    public interface IErrorHandlerService
    {
        Task HandleError(Exception ex, HttpContext context);
    }



    public class CustomErrorHandlerService : IErrorHandlerService
    {
        public async Task HandleError(Exception ex, HttpContext context)
        {
            switch (ex)
            {
                case ApiException _:
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException _:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }

            await context.Response.WriteAsync(ex.Message ?? "No error message");
        }
    }



    public class ApiException : Exception
    {
        public ApiException() : base() { }

        public ApiException(string message) : base(message) { }
    }



    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IErrorHandlerService _errorHandlerService;

        public ErrorHandlerMiddleware(RequestDelegate next, IErrorHandlerService errorHandlerService)
        {
            _next = next;
            _errorHandlerService = errorHandlerService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception error)
            {
                await _errorHandlerService.HandleError(error, httpContext);
            }
        }
    }

    public static class ErrorHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }


}
