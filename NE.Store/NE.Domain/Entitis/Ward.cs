using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Domain.Entitis
{
    public class Ward : BaseEntity
    {
        public string WardName { get; set; }
        public int DistrictId { get; set; }
        public District District { get; set; }

        //Thuộc tính liên kết với User (1:N)
        public List<User> Users { get; set; } = new();
    }
}
