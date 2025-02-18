namespace Store.Codex.APIs.Errors
{
    public class ApiValidationErrorResponse : ApiErrorResponse  
    {
        public IEnumerable<String> Errors { get; set; } = new List<String>();

        public ApiValidationErrorResponse() : base(400)
        {

        }

    }
}
