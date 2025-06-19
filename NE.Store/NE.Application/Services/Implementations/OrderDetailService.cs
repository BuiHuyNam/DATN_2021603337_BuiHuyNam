using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;
using NE.Infrastructure.Repositories.Interfaces;
using NE.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Implementations
{
    public class OrderDetailService:IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            await _unitOfWork.OrderDetails.AddAsync(orderDetail);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteOrderDetailAsync(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id);
            if (orderDetail != null)
            {
                await _unitOfWork.OrderDetails.Delete(orderDetail);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("OrderDetail does not exist!");
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailAsync()
        {
            return await _unitOfWork.OrderDetails.GetAllAsync();

        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id);
            if (orderDetail == null)
            {
                throw new Exception("OrderDetail does not exist!");
            }
            return orderDetail;
        }

        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            var orderDetailUpdate = await _unitOfWork.OrderDetails.GetByIdAsync(orderDetail.Id);
            if (orderDetailUpdate == null)
            {
                throw new Exception("OrderDetail does not exist!");
            }
            orderDetail.OrderId = orderDetailUpdate.OrderId;
            orderDetail.ProductId = orderDetailUpdate.ProductId;
            await _unitOfWork.OrderDetails.Update(orderDetail);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
