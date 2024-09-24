using Microsoft.EntityFrameworkCore;
using Tasks.DataAccess;
using Tasks.Handlers;
using Tasks.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TasksDbContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddExceptionHandler<TasksExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    using var context = scope.ServiceProvider.GetRequiredService<TasksDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseExceptionHandler();
app.UseStatusCodePages(context =>
{
    var response = context.HttpContext.Response;
    var statusCode = response.StatusCode;

    var redirectPaths = new Dictionary<int, string>
    {
        { 404, "/StatusCode/NotFoundCode/" },
        { 400, "/StatusCode/BadRequestCode/" },
        { 500, "/StatusCode/ServerErrorCode/" }
    };

    if (redirectPaths.TryGetValue(statusCode, out var path)) response.Redirect(path);

    return Task.CompletedTask;
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Task}/{action=Index}/{id?}");

app.Run();