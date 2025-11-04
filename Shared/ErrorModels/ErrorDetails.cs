using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ErrorModels
{
    public class ErrorDetails
    {
        public ErrorDetails(int statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        public int StatusCode {  get; set; }
        public string ErrorMessage {  get; set; }

        public IEnumerable<string>? Errors { get; set; }


    }

    

}
