
using Amazon.S3;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Application.Extensions;
using server.Application.Interfaces;
using server.Application.Repositories;
using server.Application.Utility.Services;
using server.Infrastructure.Auth;
using server.Infrastructure.Context;
using server.Infrastructure.Seeder;
using System.Text;

namespace server
{
    public class Program
    {
        public static async Task Main(string[] args) //void -> Task
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGenWithAuth();


            /*
                This register the DbContext, MediatR, FluentValidation, JWT, ValidationPipelineBehavior as CUSTOMIZED services to your program.
                In which they can be access through dependency injection.
            */

            //Database Context
            builder.Services.AddDbContext<ECommerceDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ECommerceConnectionString")));

            //Database Seeder
            builder.Services.AddScoped<DatabaseSeeder>();

            //MedaitR Library
            builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));

            //Custom Repository
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRespository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IFavoriteProductRepository, FavoriteProductRepository>();
            builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();


            //FluentValidation
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            //JWT (TokenGenerator)
            builder.Services.AddSingleton<TokenGenerator>(); // To be able to use the `TokenGenerator` class through dependency injection.

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero,

                        //ValidateIssuer = true,
                        //ValidateAudience = true,
                        //ValidateLifetime = true,
                        //ValidateIssuerSigningKey = true,
                    };
                });


            //builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());  
            //builder.Services.AddSingleton<IAmazonS3,AmazonS3Client>();
            //builder.Services.AddSingleton<IProductImageService,ProductImageService>();

            //ValidationPipelineBehavior
            //builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            //PROBLEM: Hindi ma control yung behavior sa pipeline.


            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seeder = services.GetRequiredService<DatabaseSeeder>();
                await seeder.Seed();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();
            app.MapControllers();
            await app.RunAsync(); //app.Run()
        }
    }
}
