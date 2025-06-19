using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.OrderDetailDto;
using NE.Application.Services.Implementations;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMapper _mapper;

        public OrderDetailController(IOrderDetailService orderDetailService, IMapper mapper)
        {
            _orderDetailService = orderDetailService;
            _mapper = mapper;
        }

        [HttpPost()]
        [Authorize]

        public async Task<ActionResult> AddOrderDetail(OrderDetailCreateDto orderDetailCreateDto)
        {
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailCreateDto);
            await _orderDetailService.AddOrderDetailAsync(orderDetail);
            return Ok(new {id = orderDetail.Id});
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllOrderDetail()
        {
            var orderDetails = await _orderDetailService.GetAllOrderDetailAsync();
            var orderDetailDto = _mapper.Map<IEnumerable<OrderDetailViewDto>>(orderDetails);
            return Ok(orderDetailDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderDetailByIdAsync(int id)
        {
            var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);
            var orderDetailDto = _mapper.Map<OrderDetailViewDto>(orderDetail);
            return Ok(orderDetailDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateOrder(OrderDetailUpdateDto orderDetailUpdateDto)
        {
            var orderDetailUpdate = _mapper.Map<OrderDetail>(orderDetailUpdateDto);
            await _orderDetailService.UpdateOrderDetailAsync(orderDetailUpdate);
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderDetail(int id)
        {
            await _orderDetailService.DeleteOrderDetailAsync(id);
            return Ok();
        }
    }
}
