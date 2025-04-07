using NE.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        //Vai tro
        IRoleRepository Roles { get; }

        //Tỉnh, huyện, xã
        IProvinceRepository Provinces { get; }
        IDistrictRepository Districts { get; }
        IWardRepository Wards { get; }

        //Danh mục, mau sac, thuong hieu
        ICategoryRepository Categories { get; }
        IColorRepository Colors { get; }
        IBrandRepository Brands { get; }

        //San pham
        IProductRepository Products { get; }
        IProductColorRepository ProductColors { get; }
        IImageFileRepository ImageFiles { get; }

        //Giam gia
        ICouponRepository Coupons { get; }

        //Nguoi dung
        IUserRepository Users { get; }

        //Gio hang , don hang
        ICartRepository Carts { get; }

        //Feedback
        IFeedbackRepository Feedbacks { get; }

        //Đơn hàng, đơn hàng chi tiết
        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }






        Task<int> SaveChangesAsync();

    }
}
