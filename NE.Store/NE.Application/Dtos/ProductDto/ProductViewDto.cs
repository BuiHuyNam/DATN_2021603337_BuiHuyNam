using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.CategoryDto;
using NE.Application.Dtos.ProductColorDto;
using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.ProductDto
{
    public class ProductViewDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public float Discount { get; set; }
        public int View { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }



        public int CategoryId { get; set; }
        public CategoryViewDto Category { get; set; }
        public int BrandId { get; set; }
        public BrandViewDto Brand { get; set; }

        public List<ProductColorViewDto> ProductColors { get; set; } = new();
        public List<ImageFile> ImageFiles { get; set; } = new();
     


    }
}
