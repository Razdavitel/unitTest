using AutoMapper;
using Domain.Sessions;
using Squads.Shared.Sessions;
using Squads.Shared.Users;
using Squads.Shared.Workouts;

namespace Squads.Server.Mapper;

public class SessionMapperProfile : Profile
{
    public SessionMapperProfile()
    {
        CreateMap<Session, SessionDto.Index>();
        //.ForMember(d => d.Workout, opt => opt.MapFrom(w => new WorkoutDto.Index() { Id = w.Workout.Id, WorkoutType = w.Workout.WorkoutType}));

        CreateMap<SessionDto.Mutate, Session>();
        CreateMap<SessionDto.MutateTrainees, Session>();

        CreateMap<Session, SessionDto.Detail>();
       /* .ForMember(d => d.Coach, opt => opt.MapFrom(s => new UserDto.Index() { FirstName = s.Coach.FirstName, LastName = s.Coach.LastName, Id = s.Coach.Id }))
         .ForMember(d => d.Workout, opt => opt.MapFrom(w => new WorkoutDto.Index() { Id = w.Workout.Id,WorkoutType=w.Workout.WorkoutType}));*/

    }
}
