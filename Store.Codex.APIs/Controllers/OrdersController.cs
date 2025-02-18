using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Codex.Core.Services.Contract;
using Store.Codex.Core;
using Microsoft.AspNetCore.Authorization;
using Store.Codex.APIs.Errors;
using System.Security.Claims;
using Store.Codex.Core.Dtos.Orders;
using Store.Codex.Core.Entities.Order;

namespace Store.Codex.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService ,IMapper mapper, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder(OrderDto model)
        { 
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var ShippedAddress = _mapper.Map<Address>(model.ShippedToAddress);

            var order = await _orderService.CreateOrderAsync(userEmail, model.BasketId , model.DeliveryMethodId, ShippedAddress);

            if (order is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrdersForSpecificUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var orders = await _orderService.GetOrderForSpecificUserAsync(userEmail);

            if (orders is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<IEnumerable<OrderToReturnDto>>(orders)); 
        }

        [HttpGet("{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrdersForSpecificUser(int orderId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var order = await _orderService.GetOrderByIdForSpecificUserAsync(userEmail,orderId);

            if (order is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }

        [HttpGet("DeliveryMethods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();

            if (deliveryMethods is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var deliveryMethodsDto = _mapper.Map<List<DeliveryMethodDto>>(deliveryMethods);

            return Ok(deliveryMethodsDto);
        }
    }
}
