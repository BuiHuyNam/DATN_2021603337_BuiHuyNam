﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Dtos.ProductDto
{
    public class ProductUpdateDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public float Discount { get; set; }


    }
}
