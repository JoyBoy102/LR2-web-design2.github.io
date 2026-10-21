using LR2WebDesign.Controllers.Interfaces;
using LR2WebDesign.Exceptions;
using LR2WebDesign.Infrastructure.Repositories;
using LR2WebDesign.Services;
using LR2WebDesign.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, InMemoryUserRepository>();

builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.MinifyCssFiles();
    pipeline.MinifyJsFiles();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = ctx =>
        {
            if (ctx.Request.Cookies.TryGetValue("accessToken", out var token))
                ctx.Token = token;
            return Task.CompletedTask;
        },

        OnChallenge = ctx =>
        {
            ctx.HandleResponse();

            if (ctx.Request.Path.StartsWithSegments("/authorization"))
                return Task.CompletedTask;

            var returnUrl = ctx.Request.Path + ctx.Request.QueryString;
            var redirect = $"/authorization?returnUrl={Uri.EscapeDataString(returnUrl)}";
            ctx.Response.Redirect(redirect);
            return Task.CompletedTask;
        }

    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseResponseCompression();

app.UseWebOptimizer();

app.UseExceptionHandler("/error");

app.Map("/error", app => app.Run(async context =>
{
    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    var status = ex switch
    {
        UserAlreadyExistsException => 409,
        GetUserException => 404,
        UserDeleteException => 400,
        UserUpdateException => 400,
        InvalidCredentialsException => 401,
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
