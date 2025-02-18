namespace Store.Codex.APIs.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiErrorResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }


        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            var message = statusCode switch
            {
                400 => "a bad request u have made",
                401 => "authorized u r not",
                404 => "resource is not found",
                500 => "server Error",  
                _ => null
            };


            return message;
        }
    }
}
