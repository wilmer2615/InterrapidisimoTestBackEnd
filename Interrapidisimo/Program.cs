using Business;
using Business.RegisteredCourseLogic;
using Business.StudentLogic;
using Business.TeacherCourseLogic;
using Interrapidisimo.Middleware;
using Microsoft.EntityFrameworkCore;
using Repository.Repository;
using Repository.Repository.RegisteredCourseRepository;
using Repository.Repository.StudentRepository;
using Repository.Repository.TeacherCourseRepository;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Interrapidisimo.Security;
using Repository.Repository.LoginRepository;
using Business.LoginLogic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configuracion Cors Origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Se realiza la configuracion de la inyección de dependencias.
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IRegisteredCourseRepository, RegisteredCourseRepository>();
builder.Services.AddScoped<ITeacherCourseRepository, TeacherCourseRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();


builder.Services.AddScoped<IStudentLogic, StudentLogic>();
builder.Services.AddScoped<IRegisteredCorseLogic, RegisteredCourseLogic>();
builder.Services.AddScoped<ITeacherCourseLogic, TeacherCourseLogic>();
builder.Services.AddScoped<ILoginLogic, LoginLogic>();

//Configuracion conexion a base de datos
var connectionString = builder.Configuration.GetConnectionString("DevConnection");

builder.Services.AddDbContext<AplicationDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


//Configuracion Jwt
builder.Services.AddScoped<IAuthentication, Authentication>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Registra el Middleware de manejo global de errores
app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowWebApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
