using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Domain.Entitis
{
    public class Product:BaseEntity
    {
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public float Discount { get; set; }
        public int View { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BrandId { get;set; }
        public Brand Brand { get;set; }

        //Thuộc tính liên kết với Feedback (1:N)
        public List<Feedback> Feedbacks { get; set; } = new();

        public List<ProductColor> ProductColors { get; set; } = new();
        public List<ImageFile> ImageFiles { get; set; } = new();
        public List<Cart> Carts { get; set; } = new();

        public List<OrderDetail> OrderDetails { get; set; } = new();




    }
}
