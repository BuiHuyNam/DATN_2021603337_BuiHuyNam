using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Domain.Entitis
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }

        //Thuộc tính liên kết với User (1:N)
        public List<User> Users { get; set; } = new();
    }
}
