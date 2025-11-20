using Azure;
using Domain.Exceptions;
using Domain.Exceptions.BadRequests;
using Shared.ErrorModels;
using Microsoft.AspNetCore.Http;
namespace Web.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // this condition will be true only if invalid endpoint route
                await HandleInvalidEndpoint(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";

                var response = new ErrorDetail()
                {
                    ErrorMessage = ex.Message,
                };

                response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    BadRequestException => StatusCodes.Status400BadRequest,
                    NotAuthorizedException => StatusCodes.Status401Unauthorized,
                    ValidationException validationException => HandleValidationExceptionErrorsList(validationException, response),
                    _ => StatusCodes.Status500InternalServerError
                };

                context.Response.StatusCode = response.StatusCode;

                await context.Response.WriteAsJsonAsync(response);
            }
        }

        private static int HandleValidationExceptionErrorsList(ValidationException validationError, ErrorDetail response)
        {
            response.Errors = validationError.Errors;
            return StatusCodes.Status401Unauthorized;
        }


        private static async Task HandleInvalidEndpoint(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var response = new ErrorDetail()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"End point {context.Request.Path} was not found"
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}

