using AutoMapper;
using DataAccess.Entities;
using DataAccess.Models.LoginUser;
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
            });
            return configuration.CreateMapper();
        }
    }
}
