
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Task_WorklogManagement.AutoMappers;
using Task_WorklogManagement.Middlewares;
using Task_WorklogManagement.Persistence;

namespace Task_WorklogManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            builder.Services.AddDbContext<TaskWorklogDbContext>(opt =>
            {
                opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"));
            });

            builder.Services.AddControllers();

            builder.Services.AddDependencyInjection(builder.Configuration);

            //JWT
            builder.Services
                .AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", opt =>
                {
                    var config = builder.Configuration;
                    var key = Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? throw new Exception("Missing Jwt: Key"));

                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                    };
                });

            // Add services to the container.
            builder.Services.AddAuthorization();

            //Swagger + Bearer
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task&WorklogManagement API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { 
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();

            app.UseMiddleware<CurrentUserMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
