using CloudinaryDotNet;
using CORE.ClaimServices;
using CORE.ItemServices;
using CORE.ReportService;
using CORE.UserServices;
using DATA.Context;
using DATA.Interface;
using DATA.Models;
using DATA.Repository;
using DATA.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddScoped<IApplicationuserServices, ApplicationuserServices>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReport, ReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IITemRepository, ItemRepository>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IClaim, ClaimRepository>();
builder.Services.AddScoped<IClaimService, ClaimService>();



builder.Services.AddDbContext<EFDbContext>(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("EFDbContext")));

//Configure SQLite connection string
//builder.Services.AddDbContext<EFDbContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("EFDbContext")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()

       .AddEntityFrameworkStores<EFDbContext>()
       .AddDefaultTokenProviders();

// Configure Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;

    options.User.RequireUniqueEmail = true;
});

// Add Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings.Key);
var cloudinaryConfig = builder.Configuration.GetSection("Cloudinary").Get<CloudinaryConfig>();

if(cloudinaryConfig == null)
{
    throw new ApplicationException("Cloudinary configuration is missing or invalid.");
}

// Configure Cloudinary
var cloudinaryAccount = new Account(
    cloudinaryConfig.CloudName,
    cloudinaryConfig.ApiKey,
    cloudinaryConfig.ApiSecret);

var cloudinary = new Cloudinary(cloudinaryAccount);

// Register Cloudinary as a singleton in the DI container
builder.Services.AddSingleton(cloudinary);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Seed data
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await EFDbContext.SeedRolesAndAdminAsync(services);
}


// Configure the HTTP request pipeline.
//if(app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//// Add Authentication
//builder.Services.AddAuthentication();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
