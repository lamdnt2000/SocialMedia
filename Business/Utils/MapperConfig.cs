using AutoMapper;
using DataAccess.Entities;
using DataAccess.Models.BranModel;
using DataAccess.Models.CategoryModel;
using DataAccess.Models.LocationModel;
using DataAccess.Models.LoginUser;
using DataAccess.Models.OrganizationModel;
using DataAccess.Models.PlatFormModel;
using DataAccess.Models.Role;

namespace Business.Utils
{
    public static class MapperConfig
    {
        public static IMapper GetMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Role, RoleDTO>().ReverseMap();
                cfg.CreateMap<Role, InsertRoleDTO>().ReverseMap();
                cfg.CreateMap<Role, UpdateRoleDTO>().ReverseMap();
                cfg.CreateMap<User, LoginUserDTO>().ReverseMap();
                cfg.CreateMap<User, InsertUserDTO>().ReverseMap();
                cfg.CreateMap<User, FacebookLoginDto>().ReverseMap();
                cfg.CreateMap<User, GoogleLoginDto>().ReverseMap();
                cfg.CreateMap<User, GoogleSignUpDto>().ReverseMap();
                cfg.CreateMap<User, FacebookSignUpDto>().ReverseMap();
                cfg.CreateMap<User, UpdateUserDto>().ReverseMap();
                cfg.CreateMap<User, UpdateUserPassworDto>().ReverseMap();

                cfg.CreateMap<Organization, InsertOrganizationDto>().ReverseMap();
                cfg.CreateMap<Organization, OrganizationDto>().ReverseMap();
                cfg.CreateMap<Organization, UpdateOrganizationDto>().ReverseMap();

                cfg.CreateMap<Brand, BrandDto>().ReverseMap();
                cfg.CreateMap<Brand, InsertBrandDto>().ReverseMap();
                cfg.CreateMap<Brand, UpdateBrandDto>().ReverseMap();

                cfg.CreateMap<Platform, PlatformDto>().ReverseMap();
                cfg.CreateMap<Platform, InsertPlatformDto>().ReverseMap();
                cfg.CreateMap<Platform, UpdatePlatformDto>().ReverseMap();

                cfg.CreateMap<Category, CategoryDto>().ReverseMap();
                cfg.CreateMap<Category, InsertCategoryDto>().ReverseMap();
                cfg.CreateMap<Category, UpdateCategoryDto>().ReverseMap();

                cfg.CreateMap<Location, LocationDto>().ReverseMap();
                cfg.CreateMap<Location, InsertLocationDto>().ReverseMap();
                cfg.CreateMap<Location, UpdateLocationDto>().ReverseMap();




            });
            return configuration.CreateMapper();
        }
    }
}
