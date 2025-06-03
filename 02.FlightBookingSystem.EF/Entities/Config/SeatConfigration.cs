using _01.FlightBookingSystem.Core.Models.Seat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.FlightBookingSystem.EF.Entities.Config
{
    public class SeatConfigration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.HasOne(s => s.Flight)
                   .WithMany(f => f.Seats)
                   .HasForeignKey(s => s.FlightID)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.Property(s => s.SeatNumber)
                .IsRequired().HasMaxLength(20);

            builder.HasCheckConstraint("CK_Seat_SeatNumber_Format", "SeatNumber LIKE '[A-Z][0-9][0-9]'");

            builder.Property(s => s.IsBooking)
                .HasDefaultValue(false);

            builder.Property(s => s.Version)
                .IsRowVersion();
        }
    }
}
