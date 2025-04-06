using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetAllCartAsync();
        Task<Cart> GetCartByIdAsync(int id);
        Task AddCartAsync(Cart cart);
        Task DeleteCartAsync(int id);
        Task UpdateCartAsync(Cart cart);
    }
}
