//last edited on 02-01-2024

using Business.Contracts;
using Business.Services;
using DataAccess.Context;
using DataAccess.Contracts;
using DataAccess.Dto.Request;
using DataAccess.Dto;
using DataAccess.Entities;
using DataAccess.Repository;
using FluentValidation;
using Punching.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Punching.Extensions;
using NLog.Web;
using Autofac.Core;

var builder = WebApplication.CreateBuilder(args);

var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
NLog.GlobalDiagnosticsContext.Set("LogDirectory", logPath);

builder.Logging.AddNLog(logPath).SetMinimumLevel(LogLevel.Trace);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Key)
    };
});

// Add Cors
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
           .SetIsOriginAllowed((host) => true);
}));

// Configure Logger service.
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<DapperContext>();
builder.Services.AddTransient<IServiceWrapper, ServiceWrapper>();
builder.Services.AddTransient<ServiceHelper>();
builder.Services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddTransient<DtoWrapper>();
builder.Services.AddTransient<ErrorResponse>();

builder.Services.AddScoped<IValidator<DailyAttendUpdateDto>, DailyAttendValidator>();
builder.Services.AddScoped<IValidator<ImageUpdateReqDto>, UpdateImageValidator>();
builder.Services.AddScoped<IValidator<EmpGetReqDto>, EmpcodeValidator>();
builder.Services.AddScoped<IValidator<HostCheckReqDto>, HostCheckValidator>();
builder.Services.AddScoped<IValidator<UserCredReqDto>, CredCheckValidator>();
builder.Services.AddScoped<IValidator<RetreiveBlobReqDto>, RetreiveBlobValidator>();

builder.Services.Configure<IISOptions>(options =>
{
    options.AutomaticAuthentication = false;
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(x =>
    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

var app = builder.Build();

// Configure the HTTP request pipeline.



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    // This middleware serves the Swagger documentation UI
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Punching API V1");
    });
}

app.UseMiddleware<ExceptionMiddleware>();

//app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("MyPolicy");

app.UseMiddleware<CorsMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();