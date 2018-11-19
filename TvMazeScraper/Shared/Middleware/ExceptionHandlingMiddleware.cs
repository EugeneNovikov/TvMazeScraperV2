using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.Wrappers;

namespace Shared.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly string InternalErrorMessage = "Error on server. Please contact administrator for details.";

        private readonly IJsonConverterWrapper _jsonConverterWrapper;

        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, IJsonConverterWrapper jsonConverterWrapper, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _jsonConverterWrapper = jsonConverterWrapper;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        public Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var exceptionMessage = InternalErrorMessage;

            switch (exception) 
            {
                case EntityNotFoundException ex:
                    code = HttpStatusCode.NotFound;
                    exceptionMessage = ex.Message;
                break;
                case TooManyRequestsException _:
                    code = HttpStatusCode.TooManyRequests;
                    break;    
                
            }

            _logger.LogError(exception.Message);

            var result = _jsonConverterWrapper.SerializeObject( exceptionMessage );
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
