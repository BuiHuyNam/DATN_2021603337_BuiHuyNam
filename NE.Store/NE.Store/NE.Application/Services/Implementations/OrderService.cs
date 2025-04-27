using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;
using NE.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order != null)
            {
                await _unitOfWork.Orders.Delete(order);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Order does not exist!");
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrderAsync()
        {
            return await _unitOfWork.Orders.GetAllAsync();

        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                throw new Exception("Order does not exist!");
            }
            return order;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            var orderUpdate = await _unitOfWork.Orders.GetByIdAsync(order.Id);
            if (orderUpdate == null)
            {
                throw new Exception("Order does not exist!");
            }
            order.CouponId = orderUpdate.CouponId;
            order.UserId = orderUpdate.UserId;
            await _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateOrderStatus(Order order)
        {
            var orderUpdate = await _unitOfWork.Orders.GetByIdAsync(order.Id);
            if(orderUpdate == null)
            {
                throw new Exception("Order does not exist");
            }

            orderUpdate.Status = order.Status;
            await _unitOfWork.Orders.Update(orderUpdate);
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
