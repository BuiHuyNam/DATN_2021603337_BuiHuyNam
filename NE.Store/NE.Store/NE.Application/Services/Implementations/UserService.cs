using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task AddUserAsync(User user)
        {
            user.RoleId = 1; //mặc định là 1
            var usersName = await _unitOfWork.Users.FindAsync(c => c.UserName == user.UserName);
            var usersEmail = await _unitOfWork.Users.FindAsync(c => c.Email == user.Email);

            if (usersName.Any() || usersEmail.Any())
            {
                throw new Exception("UserName or Email already exists!");
            }

            // ✅ Mã hóa mật khẩu
            user.Password = _passwordHasher.HashPassword(user, user.Password);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public Task DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllUserAsyns()
        {
            return await _unitOfWork.Users.GetAllAsync();

        }

        public Task<User> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
