using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;

namespace Presentation.Attributes
{
    public class CasheAttribute(int timeInSeconds) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            //logic 

          var casheservice =  context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().casheService ;

            //generate key 

            var cashekey = GetCaheKey(context.HttpContext.Request);


            var result = await casheservice.GetAsync(cashekey) ;



            if (!string.IsNullOrEmpty(result))
            {
                //return response 


                var response = new ContentResult()
                {

                    Content = result,
                    ContentType="application/json",
                    StatusCode=200
                };

                context.Result = response;
                return;




            }


       var actionContext =   await  next.Invoke();


        if(actionContext.Result is OkObjectResult okObjectResult)
            {

                await casheservice.SetAsync(cashekey, okObjectResult.Value, TimeSpan.FromSeconds(timeInSeconds));

            }




        } 

        private string GetCaheKey( HttpRequest request)
        {


            var key = new StringBuilder();

            key.Append(request.Path);

            foreach( var item in request.Query)
            {

                key.Append($"{item.Key}-{item.Value}");

               

            }

            return key.ToString();


        }

    }
}
