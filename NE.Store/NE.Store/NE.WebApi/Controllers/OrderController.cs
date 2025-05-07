using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.OrderDto;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<ActionResult> AddOrder(OrderCreateDto orderCreateDto)
        {
            var order = _mapper.Map<Order>(orderCreateDto);
            await _orderService.AddOrderAsync(order);
            return Ok(new { id = order.Id }); // Trả về JSON chứa id
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllOrder()
        {
            var orders = await _orderService.GetAllOrderAsync();
            var orderDto = _mapper.Map<IEnumerable<OrderViewDto>>(orders);
            return Ok(orderDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderByIdAsync(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            var orderDto = _mapper.Map<OrderViewDto>(order);
            return Ok(orderDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateOrder(OrderUpdateDto orderUpdateDto)
        {
            var orderUpdate = _mapper.Map<Order>(orderUpdateDto);
            await _orderService.UpdateOrderAsync(orderUpdate);
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok();
        }

        [HttpPut("UpdateOrderStatus")]
        public async Task<ActionResult> UpdateOrderStatus(UpdateOrderStatusDto updateOrderStatusDto)
        {
            var orderUpdate = _mapper.Map<Order>(updateOrderStatusDto);
            await _orderService.UpdateOrderStatus(orderUpdate);
            return Ok();
        }
    }
}
