using AutoMapper;
using DatingApp.Data;
using DatingApp.Helpers;
using DatingApp.Interfaces;
using DatingApp.Services;
using DatingApp.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            //For SignalR Configuration
            services.AddSingleton<PresenceTracker>();

            //For Cloudinary Settings
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();

            //For Database Operation
            services.AddDbContext<DataContext>(options =>
            {
                 options.UseSqlServer(config.GetConnectionString("DatingAppCon"));
                //options.UseNpgsql(config.GetConnectionString("DatingAppCon"));
            });

            //For Jwt Token Creation and Handling
            services.AddScoped<ITokenService, TokenService>();

            //For AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            //Update LastActive Property using IActionFilter
            services.AddScoped<LogUserActivity>();

            //For Repositories
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;

        }
    }
}