using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrderAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task AddOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task UpdateOrderAsync(Order order);
        Task UpdateOrderStatus(Order order);
        Task<List<Order>> GetOrderByUserIdAsync(int userId);



    }
}
