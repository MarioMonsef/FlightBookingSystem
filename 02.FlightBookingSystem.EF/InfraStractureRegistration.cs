using _01.FlightBookingSystem.Core.Interfaces;
using _01.FlightBookingSystem.Core.Services;
using _02.FlightBookingSystem.EF.Entities;
using _02.FlightBookingSystem.EF.Reposatories;
using _02.FlightBookingSystem.EF.Repositories;
using _02.FlightBookingSystem.EF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.FlightBookingSystem.EF
{
    public static class InfraStractureRegistration
    {
        public static IServiceCollection InfraStractureConfigration(this IServiceCollection services, IConfiguration configuration) {
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CS"))
                );
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFlightService, FlightService>();
            services.AddScoped<ISeatService, SeatService>();
            services.AddScoped<IBookingService, BookingService>();
            return services;
        }
    }
}
