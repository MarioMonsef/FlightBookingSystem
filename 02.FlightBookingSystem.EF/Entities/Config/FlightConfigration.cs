using _01.FlightBookingSystem.Core.DTO_s;
using _01.FlightBookingSystem.Core.Models.Flight;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.FlightBookingSystem.EF.Entities.Config
{
    public class FlightConfigration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.Property(f => f.DepartureTime)
                .IsRequired().HasColumnType("datetime2");
            builder.Property(f=>f.ArrivalTime)
                .IsRequired().HasColumnType("datetime2");
            builder.Property(f=>f.ArrivalCity)
                .IsRequired().HasMaxLength(100);
            builder.Property(f=>f.DepartureCity)
                .IsRequired().HasMaxLength(100);
            builder.Property(f=>f.Price)
                .IsRequired().HasColumnType("decimal(18,2)")
                .HasPrecision(18,2);

            builder.HasCheckConstraint("CK_Flight_Price", "Price >= 0");

            builder.Property(f=>f.FlightNumber)
                .IsRequired().HasMaxLength(20);
            
        }
    }
}
