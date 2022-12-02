using Persistence.Data;
using Squads.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

// setup db connection, context, config
builder.Services.AddDbContextAndServices<SquadContext>(
    builder.Configuration,
    builder.Environment
);

// Add services to the container.
builder.Services.LoadConfiguration(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddServices();
builder.Services.AddDbSeedServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerService();
builder.Services.AddAutoMapperServices();
builder.Services.AddAuthenticationService(builder.Configuration);
builder.Services.AddAllDbServices();

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<SquadDataInitializer>()?.SeedData();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
