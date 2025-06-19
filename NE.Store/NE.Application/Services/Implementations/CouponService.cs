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
    public class CouponService : ICouponService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CouponService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddCouponAsync(Coupon coupon)
        {
            var coupons = await _unitOfWork.Coupons.FindAsync(c => c.Code == coupon.Code);
            if (coupons.Any())
            {
                throw new Exception("Coupon already exists!");
            }
            await _unitOfWork.Coupons.AddAsync(coupon);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCouponAsync(int id)
        {
            var coupon = await _unitOfWork.Coupons.GetByIdAsync(id);
            if (coupon != null)
            {
                await _unitOfWork.Coupons.Delete(coupon);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Coupon does not exist!");
            }
        }

        public async Task<IEnumerable<Coupon>> GetAllCouponAsync()
        {
            return await _unitOfWork.Coupons.GetAllAsync();

        }

        public async Task<Coupon> GetCouponByIdAsync(int id)
        {
            var coupon = await _unitOfWork.Coupons.GetByIdAsync(id);
            if (coupon == null)
            {
                throw new Exception("Coupon does not exist!");
            }
            return coupon;
        }

        public async Task UpdateCouponAsync(Coupon coupon)
        {
            var couponUpdate = await _unitOfWork.Coupons.FindAsync(c => c.Id == coupon.Id);

            if (!couponUpdate.Any())
            {
                throw new Exception("Coupon does not exist!");
            }

            //var coupons = await _unitOfWork.Coupons.FindAsync(c => c.Code == coupon.Code);
            //if (coupons.Any())
            //{
            //    throw new Exception("Coupon already exists!");
            //}

            await _unitOfWork.Coupons.Update(coupon);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
