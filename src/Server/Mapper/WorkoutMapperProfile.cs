using AutoMapper;
using Domain.Sessions;
using Domain.Workouts;
using Squads.Shared.Sessions;
using Squads.Shared.Users;
using Squads.Shared.Workouts;

namespace Squads.Server.Mapper;

public class WorkoutMapperProfile : Profile
{
    public WorkoutMapperProfile()
    {
        CreateMap<WorkoutDto.Mutate, Workout>();

        CreateMap<Workout, WorkoutDto.Detail>();

        CreateMap<Workout, WorkoutDto.Index>();


        //.ForMember(d => d.Coach, opt => opt.MapFrom(s => new UserDto.Detail() { FirstName = s.Coach.FirstName, LastName = s.Coach.LastName, Id = s.Coach.Id }));

    }
}
