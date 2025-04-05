using AutoMapper;
using NE.Application.Dtos.CategoryDto;
using NE.Application.Dtos.ColorDto;
using NE.Application.Dtos.DistrictDto;
using NE.Application.Dtos.ProvinceDto;
using NE.Application.Dtos.RoleDto;
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







        }
    }
}
