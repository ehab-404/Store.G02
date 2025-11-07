using Azure.Core;
using Domain.Exceptions;
using Domain.Exceptions.BadRequests;
using Shared.ErrorModels;

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
                await _next.Invoke(context);

                if(context.Response.StatusCode==StatusCodes.Status404NotFound)
                {
                    await HandlingNotFoundEndPointAsync(context);

                }

            }
            catch (Exception ex)
            {
                //log exception    for admin , maintance  

                _logger.LogError(ex.Message, ex);

                await HandlingErrorAsync(context, ex);

            }
        }

        private static async Task HandlingErrorAsync(HttpContext context, Exception ex)
        {


            if (context.Response.HasStarted) { Console.WriteLine("can not modify headers as response already started");
                return;
            
            }







            //1.set status code for response 
            //2.set content type for response 
            //3.ResponseObject(Body)
            //4.Return Response 

             context.Response.StatusCode= StatusCodes.Status500InternalServerError;

          context.Response.ContentType = ContentType.ApplicationJson.ToString();


            // 

            var response = new ErrorDetails(context.Response.StatusCode, ex.Message);
            

            response.StatusCode = ex switch
            {

                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                ValidationException=>HandlingVAlidationExceptionAsync((ValidationException) ex , response),
                UnAuthorizedException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError


            };
            

            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandlingNotFoundEndPointAsync(HttpContext context)
        {
            context.Response.ContentType = ContentType.ApplicationJson.ToString();
            var response = new ErrorDetails(context.Response.StatusCode,

                $"End Point of path {context.Request.Path} Is Not Found"
             );


            await context.Response.WriteAsJsonAsync(response);

        }
        
        
        private static int HandlingVAlidationExceptionAsync(ValidationException exception , ErrorDetails response)
        {


            response.Errors = exception.Errors;

            return StatusCodes.Status400BadRequest;



        }
    }
}
