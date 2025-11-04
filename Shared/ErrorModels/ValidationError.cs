namespace Shared.ErrorModels
{
    public class ValidationError
    {

        public string FieldName { get; set; } 


        public IEnumerable<string> Errors { get; set; }


    }
}