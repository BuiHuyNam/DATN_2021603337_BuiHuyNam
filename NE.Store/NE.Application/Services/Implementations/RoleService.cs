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
    public class RoleService :IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddRoleAsync(Role role)
        {
            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(int id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            if (role != null)
            {
                await _unitOfWork.Roles.Delete(role);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Role does not exist!");
            }
        }

        public async Task<IEnumerable<Role>> GetAllRoleAsync()
        {
            return await _unitOfWork.Roles.GetAllAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            if (role == null)
            {
                throw new Exception("Role does not exist!");
            }
            return role;
        }

        public async Task UpdateRoleAsync(Role role)
        {
            var roleUpdate = await _unitOfWork.Roles.GetByIdAsync(role.Id);
            if (roleUpdate == null)
            {
                throw new Exception("Role does not exist!");
            }
            await _unitOfWork.Roles.Update(role);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
