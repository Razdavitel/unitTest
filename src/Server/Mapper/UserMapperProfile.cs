using AutoMapper;
using Domain.Common;
using Domain.Users;
using Squads.Shared.Users;

namespace Squads.Server.Mapper;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserDto.Index>();

        CreateMap<User, UserDto.Detail>();

        CreateMap<UserDto.Mutate, User>()
            .ForMember(u => u.Role, u => u.MapFrom(us => us.Role ?? RoleType.Customer));

        CreateMap<UserDto.Create, User>()
            .AfterMap((c, u) => u.Role = RoleType.Customer);
    }
}
