using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Store.Codex.APIs.Errors;
using Store.Codex.Repository.Data.Contexts;

namespace Store.Codex.APIs.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreDbContext _Context;
        public BuggyController(StoreDbContext context)
        {
            _Context = context;
        }

        [HttpGet("notfound")] // GET : /api/Buggy/notfound
        public async Task<IActionResult> GetNotFoundRequestError()
        {
            var brand = await _Context.Brands.FindAsync(100);

            if (brand is null)
            {
                return NotFound(new ApiErrorResponse(404));
            }
            return Ok(brand);
        }

        [HttpGet("servererror")] // GET : /api/Buggy/servererror
        // Server is an error as result of a exception in the backend 
        public async Task<IActionResult> GetServerError()
        {
            var brand = await _Context.Brands.FindAsync(100);

            var brandToString = brand.ToString(); // will Throw Exception (Null Reference Exception)

            return Ok(brand);
        }

        [HttpGet("badrequest")] // GET : /api/Buggy/badrequest
        public async Task<IActionResult> GetBadRequestError()
        {
            return BadRequest(new ApiErrorResponse(400));
        }

        [HttpGet("badrequest/{id}")] // GET : /api/Buggy/badrequest/ahmed 
        public async Task<IActionResult> GetBadRequestError(int id) // validatoin error 
        {
            return Ok(); // it did not get into the function -> model binding fails in ASP.NET Core can not mathc the incoming request So I Need to coustomize the configuration of the service that is responsible to generate me this response and return the response i want
        }

        [HttpGet("unauthorized")] // GET : /api/Buggy/unauthorized
        public async Task<IActionResult> GetUnauthorizedError(int id) // unauthorized person
        {
            return Unauthorized(new ApiErrorResponse(401));
        }
    }
}
