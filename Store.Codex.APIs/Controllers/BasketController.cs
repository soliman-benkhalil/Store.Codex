using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Codex.APIs.Errors;
using Store.Codex.Core.Dtos.Basket;
using Store.Codex.Core.Entities;
using Store.Codex.Core.Repositories.Contract;
using Store.Codex.Core.Services.Contract;

namespace Store.Codex.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : BaseApiController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("{id}")] // GET : /api/basket/basket01
        public async Task<IActionResult> GetBasketById(string? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400, "Invalid Id !"));

            var basket = await _basketService.GetBasketAsync(id);

            if (basket is null)
            {
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
            }

            return Ok(basket);
        }

        [HttpPost] // POST : /api/basket
        public async Task<IActionResult> CreateOrUpdateBasket(CustomerBasketDto model)
        {
            if (model is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var basket = await _basketService.UpdateBasketAsync(model);

            if (basket is null)
            {
                return BadRequest(new ApiErrorResponse(400));
            }
            return Ok(basket);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBasket(string id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400));

            var flag = await _basketService.DeleteBasketAsync(id);

            if (!flag) return BadRequest(new ApiErrorResponse(400));

            return NoContent();
        }
    }
}
