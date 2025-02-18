using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Codex.APIs.Errors;

namespace Store.Codex.APIs.Controllers
{
    [Route("error/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] // not i ignored the endPoints in this controller as like the application does not c ( because we did not determine the verb )it and it only applicable in case of error
    public class ErrorsController : ControllerBase
    {
        // the swagger has an error because i did not determine the verb of this endpoint
        public IActionResult Error(int code)
        {
            return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound , "Not Found End Point !"));
        }
    }
}
