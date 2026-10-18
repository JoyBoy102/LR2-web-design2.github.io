using LR2WebDesign.Controllers.Interfaces;
using LR2WebDesign.Exceptions;
using LR2WebDesign.Infrastructure.Repositories;
using LR2WebDesign.Services;
using LR2WebDesign.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, InMemoryUserRepository>();

var app = builder.Build();


app.UseExceptionHandler("/error");

app.Map("/error", app => app.Run(async context =>
{
    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    var status = ex switch
    {
        UserAlreadyExistsException => 409,
        GetUserException => 404,
        UserDeleteException => 400,
        _ => 500
    };

    context.Response.StatusCode = status;
    var errorMessage = status == 500
    ? "Внутренняя ошибка сервера"
    : ex?.Message ?? "Неизвестная ошибка";

    var errorResponse = new
    {
        errors = new List<string> { errorMessage },
        status = status
    };

    await context.Response.WriteAsJsonAsync(errorResponse);
}));

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
