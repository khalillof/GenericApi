using GenericApi;
using GenericApi.Data;
using GenericApi.Services;
using Microsoft.EntityFrameworkCore;
using GenericApi.Jwt;
using Microsoft.OpenApi.Models;
using GenericApi.Models;
using Microsoft.AspNetCore.Identity;

var LocalOrigins = "local";

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// add databse provider
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("generic-api-database"));

// For Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// add JwtSetings and AddAuthentication and AddJwtBearer
builder.Services.AddJWTTokenServices(builder.Configuration);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: LocalOrigins,
                      policy =>
                      {
                          //policy.WithOrigins("http://localhost:7064","https://localhost:7064").AllowAnyHeader().AllowAnyMethod(); 
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
});
// Add data services.
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // seeding database
    SeedData.EnsureSeedData(app.Services);
}

app.UseHttpsRedirection();
app.UseCors(LocalOrigins);

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
