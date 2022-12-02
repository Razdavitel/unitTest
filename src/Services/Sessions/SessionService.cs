using Ardalis.GuardClauses;
using AutoMapper;
using Domain.Sessions;
using Domain.Users;
using Domain.Workouts;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Extensions;
using Squads.Shared.Sessions;
using Squads.Shared.Users;
using Squads.Shared.Workouts;

namespace Services.Sessions;
public class SessionService : ISessionService
{
    private readonly SquadContext DbCtx;
    private readonly DbSet<Session> sessions;
    private readonly DbSet<User> users;
    private readonly DbSet<Workout> workouts;
    private readonly IMapper mapper;

    public SessionService(SquadContext dbCtx, IMapper mapper)
    {
        DbCtx = dbCtx;
        sessions = dbCtx.Sessions;
        users = dbCtx.Users;
        workouts = dbCtx.Workouts;
        this.mapper = mapper;
    }

    private IQueryable<Session> GetSessionById(int id) => sessions
     .AsNoTracking().Include(s => s.Trainees)
        .Include(s => s.Coach)
        .Include(s => s.Workout)
        .Include(s => s.Trainees)
     .Where(u => u.Id == id);
    private IQueryable<User> GetUserById(int id) => users
    .AsNoTracking()
    .Where(u => u.Id == id);
    private IQueryable<Workout> GetWorkoutById(int id) => workouts
   .AsNoTracking()
   .Where(w => w.Id == id);

    public async Task<SessionResponse.Create> CreateAsync(SessionRequest.Create req)
    {
        SessionResponse.Create res = new();
        var user = await users.SingleOrDefaultAsync(u => u.Id == req.Session.CoachId);
        var workout = await workouts.SingleOrDefaultAsync(u => u.Id == req.Session.WorkoutId);
        Guard.Against.Null(user, nameof(user));
        Guard.Against.Null(workout, nameof(workout));

        var newSession = mapper.Map<Session>(
            req.Session,
            opt => opt.AfterMap((src, dest) =>
            {
                dest.Coach = user;
                dest.Workout = workout;
            })

        );

        var session = sessions.Add(newSession);

        await DbCtx.SaveChangesAsync();
        res.SessionId = session.Entity.Id;
        return res;
    }

    public async Task DeleteAsync(SessionRequest.Delete req)
    {
        sessions.RemoveIf(p => p.Id == req.SessionId);
        await DbCtx.SaveChangesAsync();
    }

    public async Task<SessionResponse.Edit> EditAsync(SessionRequest.Edit req)
    {

        SessionResponse.Edit res = new();
        var session = await GetSessionById(req.SessionId).SingleOrDefaultAsync();
        var coach = await GetUserById((int)req.Session.CoachId).SingleOrDefaultAsync();
        var workout = await GetWorkoutById((int)req.Session.WorkoutId).SingleOrDefaultAsync();
        var model = req.Session;
        Guard.Against.Null(session, nameof(session));
        Guard.Against.Null(coach, nameof(coach));
        Guard.Against.Null(workout, nameof(workout));
        session.Description = model.Description;
        session.EndsAt = model.EndsAt;
        session.StartsAt = model.StartsAt;
        session.Title = model.Title;
        session.Coach = coach;
        session.Workout = workout;

        DbCtx.Entry(session).State = EntityState.Modified;

        await DbCtx.SaveChangesAsync();

        res.SessionId = session.Id;

        return res;
    }


    public async Task<SessionResponse.GetDetail> GetDetailAsync(SessionRequest.GetDetail req)
    {
        SessionResponse.GetDetail res = new();
        var session = await GetSessionById(req.SessionId)
            .Select(x => x)
            .SingleOrDefaultAsync();
        Guard.Against.Null(session, nameof(session));
        List<UserDto.Index> trainees = session.Trainees.Select(t => mapper.Map<UserDto.Index>(t)).ToList();
        var sessionDetail = new SessionDto.Detail()
        {
            Id = session.Id,
            Workout = mapper.Map<WorkoutDto.Index>(session.Workout),
            Coach = mapper.Map<UserDto.Index>(session.Coach),
            StartsAt = session.StartsAt,
            EndsAt = session.EndsAt,
            Title = session.Title,
            Description = session.Description,
            Trainees = trainees
        };
        res.Session = sessionDetail;
        return res;
    }

    public async Task<SessionResponse.GetIndex> GetIndexAsync(SessionRequest.GetIndex req)
    {
        SessionResponse.GetIndex res = new();
        var query = sessions.Include(s => s.Workout).Include(s => s.Trainees)
            .AsQueryable().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(req.Description))
            query = query.Where(x => x.Description.Contains(req.Description));

        res.TotalAmount = query.Count();

        if (req.Amount != null && req.Page != null)
        {
            query = query.Take(req.Amount ?? 0);
            query = query.Skip(req.Amount * req.Page ?? 0);
        }

        query.OrderBy(x => x.StartsAt);

        res.Sessions = await query
            .Select(x => mapper.Map<SessionDto.Index>(x))
            .ToListAsync();

        return res;
    }

    public async Task<SessionResponse.MutateTrainee> AddTrainee(SessionRequest.MutateTrainee request)
    {
        SessionResponse.MutateTrainee res = new();
        var session = await DbCtx.Sessions
                          .Include(s => s.Trainees)
                          .FirstOrDefaultAsync(x => x.Id == request.SessionId);
        var user = await GetUserById(request.MutateTrainees.TraineeId).FirstOrDefaultAsync();

        Guard.Against.Null(session, nameof(session));
        Guard.Against.Null(user, nameof(user));

        session.AddReservation(user);

        await DbCtx.SaveChangesAsync();

        res.SessionId = session.Id;

        return res;
    }

    public async Task<SessionResponse.MutateTrainee> RemoveTrainee(SessionRequest.MutateTrainee request)
    {
        SessionResponse.MutateTrainee res = new();
        var session = await DbCtx.Sessions
                          .Include(s => s.Trainees)
                          .FirstOrDefaultAsync(x => x.Id == request.SessionId);

        var user = await GetUserById(request.MutateTrainees.TraineeId).FirstOrDefaultAsync();

        Guard.Against.Null(session, nameof(session));
        Guard.Against.Null(user, nameof(user));

        session.RemoveReservation(user);

        await DbCtx.SaveChangesAsync();

        res.SessionId = session.Id;

        return res;
    }
}
