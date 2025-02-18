namespace Store.Codex.APIs.Errors
{
    public class ApiExceptionResponse : ApiErrorResponse
    {
        public string? Details { get; set; }

        public ApiExceptionResponse(int statucCode,string? message = null ,string? details = null) : base(statucCode, message)
        {
            Details = details;
        }
    }
}
