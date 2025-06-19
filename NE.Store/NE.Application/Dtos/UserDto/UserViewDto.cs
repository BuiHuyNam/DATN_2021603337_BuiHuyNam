using NE.Application.Dtos.RoleDto;
using NE.Application.Dtos.WardDto;
using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.UserDto
{
    public class UserViewDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? AddressDetail { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsActive { get; set; }


        public int RoleId { get; set; }
        public RoleViewDto Role { get; set; }

        public int? WardId { get; set; }
        public WardViewDto Ward { get; set; }


    }
}
