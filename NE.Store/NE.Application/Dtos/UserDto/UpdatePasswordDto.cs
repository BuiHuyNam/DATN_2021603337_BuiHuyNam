using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.UserDto
{
    public class UpdatePasswordDto
    {
        public int Id { get; set; }
        public string Password { get; set; }

        public string NewPassword { get; set; }
        public string ComfirmPassword { get; set; }
    }
}
