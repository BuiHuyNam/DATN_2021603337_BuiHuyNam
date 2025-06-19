using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Interfaces
{
    public interface ICouponService
    {
        Task<IEnumerable<Coupon>> GetAllCouponAsync();
        Task<Coupon> GetCouponByIdAsync(int id);
        Task AddCouponAsync(Coupon coupon);
        Task DeleteCouponAsync(int id);
        Task UpdateCouponAsync(Coupon coupon);
    }
}
