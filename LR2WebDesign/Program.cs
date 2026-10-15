using LR2WebDesign.Controllers.Interfaces;
using LR2WebDesign.Infrastructure.Repositories;
using LR2WebDesign.Services;
using LR2WebDesign.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, InMemoryUserRepository>();

var app = builder.Build();



app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
