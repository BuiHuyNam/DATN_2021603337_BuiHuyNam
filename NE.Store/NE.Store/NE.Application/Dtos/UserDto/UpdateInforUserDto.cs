using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.UserDto
{
    public class UpdateInforUserDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AddressDetail { get; set; }
        public int? WardId { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }
}
