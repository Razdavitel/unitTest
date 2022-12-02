using Ardalis.GuardClauses;
using AutoMapper;
using Domain.Workouts;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Extensions;
using Squads.Shared.Workouts;

namespace Services.Workouts;
public class WorkoutService : IWorkoutService
{
    private readonly SquadContext DbCtx;
    private readonly DbSet<Workout> workouts;
    private readonly IMapper mapper;

    public WorkoutService(SquadContext dbCtx, IMapper mapper)
    {
        DbCtx = dbCtx;
        workouts = dbCtx.Workouts;      
        this.mapper = mapper;
    }

    private IQueryable<Workout> GetWorkoutById(int id) => workouts
         .AsNoTracking()
         .Where(w => w.Id == id);


    public async Task<WorkoutResponse.Create> CreateAsync(WorkoutRequest.Create request)
    {
        WorkoutResponse.Create res = new ();
        var newWorkout = mapper.Map<Workout>(request.Workout);
        var workout = workouts.Add(newWorkout);
        await DbCtx.SaveChangesAsync();
        res.WorkoutId = workout.Entity.Id;
        return res;
    }

    public async Task DeleteAsync(WorkoutRequest.Delete request)
    {
        workouts.RemoveIf(p => p.Id == request.WorkoutId);
        await DbCtx.SaveChangesAsync();
    }

    public async Task<WorkoutResponse.Edit> EditAsync(WorkoutRequest.Edit request)
    {
        WorkoutResponse.Edit res = new();
        var workout = await GetWorkoutById(request.WorkoutId).SingleOrDefaultAsync();

        Guard.Against.Null(workout, nameof(workout));

        var model = request.Workout;

        workout.Date = model.Date;
        workout.WorkoutType = model.WorkoutType;

        DbCtx.Entry(workout).State = EntityState.Modified;
        await DbCtx.SaveChangesAsync();
        res.WorkoutId = workout.Id;
        return res; 
    }

    public async Task<WorkoutResponse.GetDetail> GetDetailAsync(WorkoutRequest.GetDetail request)
    {
        WorkoutResponse.GetDetail res = new();
        var workout =  await GetWorkoutById(request.WorkoutId)
            .Select(x => mapper.Map<WorkoutDto.Detail>(x)) 
            .SingleOrDefaultAsync();
        Guard.Against.Null(workout, nameof(workout));
        res.Workout = workout;
        return res;
    }

    public async Task<WorkoutResponse.GetIndex> GetIndexAsync(WorkoutRequest.GetIndex request)
    {
        WorkoutResponse.GetIndex res = new();
        var query = workouts.AsQueryable().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(x => x.Date.ToString().Contains(request.SearchTerm));

        res.TotalAmount = query.Count();

        if(request.Amount != null && request.Page != null)
        {
            query = query.Take(request.Amount ?? 0);
            query = query.Skip(request.Amount * request.Page ?? 0);
        }

        query.OrderBy(x => x.Date);
        res.Workouts = await query.Select(x => mapper.Map<WorkoutDto.Index>(x)).ToListAsync();

        return res;
    }
}
