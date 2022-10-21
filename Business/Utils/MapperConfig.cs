using AutoMapper;
using DataAccess.Entities;
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
            });
            return configuration.CreateMapper();
        }
    }
}
