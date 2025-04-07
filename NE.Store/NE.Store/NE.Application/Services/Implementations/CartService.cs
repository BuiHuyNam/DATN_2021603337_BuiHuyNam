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
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCartAsync(int id)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(id);
            if (cart != null)
            {
                await _unitOfWork.Carts.Delete(cart);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Cart does not exist!");
            }
        }

        public async Task<IEnumerable<Cart>> GetAllCartAsync()
        {
            return await _unitOfWork.Carts.GetAllAsync();

        }

        public async Task<Cart> GetCartByIdAsync(int id)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(id);
            if (cart == null)
            {
                throw new Exception("Cart does not exist!");
            }
            return cart;
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            var cartUpdate = await _unitOfWork.Carts.GetByIdAsync(cart.Id);
            if (cartUpdate == null)
            {
                throw new Exception("Cart does not exist!");
            }
            cart.ProductId = cartUpdate.ProductId;
            cart.UserId = cartUpdate.UserId;
            await _unitOfWork.Carts.Update(cart);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
