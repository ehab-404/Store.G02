using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ErrorModels
{
    public class ValidationErrorResponse
    {
        public int StatusCode { get; set; } = 400;

        public string ErrorMessage { get; set; } = "Validation Errors ";

        public IEnumerable<ValidationError> Errors { get; set; }


    }
}
