using NE.Infrastructure.Context;
using NE.Infrastructure.Repositories.Implementations;
using NE.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly NEContext _context;
        public IRoleRepository Roles { get; }
        public IProvinceRepository Provinces { get; }
        public IDistrictRepository Districts { get; }
        public IWardRepository Wards { get; }
        public ICategoryRepository Categories { get; }
        public IColorRepository Colors { get; }
       


        public UnitOfWork(NEContext context)
        {
            _context = context;
            Roles = new RoleRepository(_context);
            Provinces = new ProvinceRepository(_context);
            Districts = new DistrictRepository(_context);
            Wards = new WardRepository(_context);
            Categories = new CategoryRepository(_context);
            Colors = new ColorRepository(_context);

        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();

        }
    }
}
