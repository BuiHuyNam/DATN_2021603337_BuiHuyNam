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




        Task<int> SaveChangesAsync();

    }
}
