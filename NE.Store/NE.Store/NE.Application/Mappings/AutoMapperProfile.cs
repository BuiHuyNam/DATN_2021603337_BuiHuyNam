using AutoMapper;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.CartDto;
using NE.Application.Dtos.CategoryDto;
using NE.Application.Dtos.ColorDto;
using NE.Application.Dtos.CouponDto;
using NE.Application.Dtos.DistrictDto;
using NE.Application.Dtos.Feedback;
using NE.Application.Dtos.ImageFileDto;
using NE.Application.Dtos.OrderDetailDto;
using NE.Application.Dtos.OrderDto;
using NE.Application.Dtos.ProductColorDto;
using NE.Application.Dtos.ProductDto;
using NE.Application.Dtos.ProvinceDto;
using NE.Application.Dtos.RoleDto;
using NE.Application.Dtos.UserDto;
using NE.Application.Dtos.WardDto;
using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Mappings
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Role
            CreateMap<Role, RoleCreateDto>().ReverseMap();
            CreateMap<Role, RoleUpdateDto>().ReverseMap();
            CreateMap<Role, RoleViewDto>().ReverseMap();

            //Province
            CreateMap<Province, ProvinceCreateDto>().ReverseMap();
            CreateMap<Province, ProvinceUpdateDto>().ReverseMap();
            CreateMap<Province, ProvinceViewDto>().ReverseMap();

            //District
            CreateMap<District, DistrictCreateDto>().ReverseMap();
            CreateMap<District, DistrictUpdateDto>().ReverseMap();
            CreateMap<District, DistrictViewDto>().ReverseMap();

            //Ward
            CreateMap<Ward, WardCreateDto>().ReverseMap();
            CreateMap<Ward, WardUpdateDto>().ReverseMap();
            CreateMap<Ward, WardViewDto>().ReverseMap();

            //Category
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Category, CategoryViewDto>().ReverseMap();

            //Color
            CreateMap<Color, ColorUpdateDto>().ReverseMap();
            CreateMap<Color, ColorCreateDto>().ReverseMap();
            CreateMap<Color, ColorViewDto>().ReverseMap();

            //Brand 
            CreateMap<Brand, BrandCreateDto>().ReverseMap();
            CreateMap<Brand, BrandUpdateDto>().ReverseMap();
            CreateMap<Brand, BrandViewDto>().ReverseMap();
            CreateMap<Brand, IsActiveBrandDto>().ReverseMap();

            //Product
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Product, ProductViewDto>().ReverseMap();


            //ProductColor
            CreateMap<ProductColor, ProductColorCreateDto>().ReverseMap();
            CreateMap<ProductColor, ProductColorUpdateDto>().ReverseMap();
            CreateMap<ProductColor, ProductColorViewDto>().ReverseMap();

            //ImageFile
            CreateMap<ImageFile, ImageFileDto>().ReverseMap();

            //Coupon
            CreateMap<Coupon, CouponCreateDto>().ReverseMap();
            CreateMap<Coupon, CouponViewDto>().ReverseMap();
            CreateMap<Coupon, CouponUpdateDto>().ReverseMap();

            //User
            CreateMap<User, UserCreateDto>().ReverseMap();

            //Cart
            CreateMap<Cart, CartUpdateDto>().ReverseMap();
            CreateMap<Cart, CartCreateDto>().ReverseMap();
            CreateMap<Cart, CartViewDto>().ReverseMap();

            //Feedback
            CreateMap<Feedback, FeedbackCreateDto>().ReverseMap();
            CreateMap<Feedback, FeedbackUpdateDto>().ReverseMap();
            CreateMap<Feedback, FeedbackViewDto>().ReverseMap();

            //Order
            CreateMap<Order, OrderCreateDto>().ReverseMap();
            CreateMap<Order, OrderUpdateDto>().ReverseMap();
            CreateMap<Order, OrderViewDto>().ReverseMap();

            CreateMap<OrderDetail, OrderDetailCreateDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailUpdateDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailViewDto>().ReverseMap();












        }
    }
}
