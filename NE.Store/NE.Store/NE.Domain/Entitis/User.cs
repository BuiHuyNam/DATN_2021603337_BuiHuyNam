using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Domain.Entitis
{
    public class User :BaseEntity
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AddressDetail { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int WardId { get; set; }
        public Ward Ward { get; set; }

        //Thuộc tính liên kết với Feedback (1:N)
        public List<Feedback> Feedbacks { get; set; } = new();
        public List<Cart> Carts { get; set; } = new();
        public List<Order> Orders { get; set; } = new();




    }
}
