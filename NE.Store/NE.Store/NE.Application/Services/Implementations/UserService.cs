using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using NE.Application.Dtos.AuthDto;
using NE.Application.Dtos.UserDto;
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

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User does not exist!");
            }
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            var userUpdate = await _unitOfWork.Users.GetByIdAsync(user.Id);
            if (userUpdate == null)
            {
                throw new Exception("User does not exist!");
            }
            userUpdate.DateOfBirth = user.DateOfBirth;
            userUpdate.AddressDetail = user.AddressDetail;
            userUpdate.FullName = user.FullName;
            userUpdate.PhoneNumber = user.PhoneNumber;
            userUpdate.WardId = user.WardId;

            await _unitOfWork.Users.Update(userUpdate);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task IsActiveUser(User user)
        {
            var userUpdate = await _unitOfWork.Users.GetByIdAsync(user.Id);

            if (userUpdate == null)
            {
                throw new Exception("user does not exist!");
            }

            if (userUpdate.IsActive == true)
            {
                userUpdate.IsActive = false;
            }
            else
            {
                userUpdate.IsActive = true;
            }

            await _unitOfWork.Users.Update(userUpdate);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            var userUpdate = await _unitOfWork.Users.GetByIdAsync(updatePasswordDto.Id);
            if (userUpdate == null)
            {
                throw new Exception("User does not exist!");
            }

            // ✅ So sánh mật khẩu cũ
            var result = _passwordHasher.VerifyHashedPassword(userUpdate, userUpdate.Password, updatePasswordDto.Password);
            if (result != Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
            {
                throw new Exception("Sai mật khẩu cũ!");
            }

            // ✅ Kiểm tra xác nhận mật khẩu mới
            if (updatePasswordDto.NewPassword != updatePasswordDto.ComfirmPassword)
            {
                throw new Exception("Mật khẩu xác nhận không khớp!");
            }

            // ✅ Mã hóa và cập nhật mật khẩu mới
            var hashedNewPassword = _passwordHasher.HashPassword(userUpdate, updatePasswordDto.NewPassword);
            userUpdate.Password = hashedNewPassword;

            await _unitOfWork.Users.Update(userUpdate);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
