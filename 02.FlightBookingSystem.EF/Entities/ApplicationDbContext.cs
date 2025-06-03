using _01.FlightBookingSystem.Core.Models.Booking;
using _01.FlightBookingSystem.Core.Models.Flight;
using _01.FlightBookingSystem.Core.Models.Identity;
using _01.FlightBookingSystem.Core.Models.Seat;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _02.FlightBookingSystem.EF.Entities
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(assembly : Assembly.GetExecutingAssembly());
        }
        public virtual DbSet<Flight> flights { get; set; }
        public virtual DbSet<Seat> Seats { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }


    }
}
