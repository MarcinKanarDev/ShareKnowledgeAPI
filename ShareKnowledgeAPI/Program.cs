using Microsoft.EntityFrameworkCore;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Implementation;
using ShareKnowledgeAPI.Mapper;
using ShareKnowledgeAPI.Middleware;
using ShareKnowledgeAPI.Seeder;
using ShareKnowledgeAPI.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

//Add custom Middlewares
builder.Services.AddScoped<ErrorHandlingMiddleware>();

//Database context service
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("ShareKnowledgeApi")));

//Services
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<DataSeeder>();

//add mapper service
builder.Services.AddAutoMapper(typeof(ApplicationMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
