using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShareKnowledgeAPI.Authentication;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Implementation;
using ShareKnowledgeAPI.Mapper;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Validators;
using ShareKnowledgeAPI.Middleware;
using ShareKnowledgeAPI.Services;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using ShareKnowledgeAPI.Authorization;
using ShareKnowledgeAPI.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Add authentication settings
var authenticationSettings = new AuthenticationSettings();

configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        RequireExpirationTime = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", builder =>
        builder.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins(configuration["AllowedOrigins"])
    );
});
//Add custom Middlewares
builder.Services.AddScoped<ErrorHandlingMiddleware>();

//Database context service
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("ShareKnowledgeApi")));

//Services
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor();
//Add password hasher to the container
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

//Add Authorization
builder.Services.AddScoped<IAuthorizationHandler, PostOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CommentOperationRequirementHandler>();

//Add Validators to the container
builder.Services.AddScoped<IValidator<UserRegisterDto>, UserRegisterValidator>();
builder.Services.AddScoped<IValidator<Query>, QueryValidator>();
builder.Services.AddScoped<IValidator<CreatePostDto>, CreatePostValidator>();

//add mapper service
builder.Services.AddAutoMapper(typeof(ApplicationMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("FrontEndClient");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();

public partial class Program 
{ }
