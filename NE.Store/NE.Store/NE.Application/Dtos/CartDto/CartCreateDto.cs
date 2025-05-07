using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.CartDto
{
    public class CartCreateDto
    {
        public int Quatity { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
    }
}
