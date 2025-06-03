using _01.FlightBookingSystem.Core.Models.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.FlightBookingSystem.EF.Entities.Config
{
    public class BookingConfigration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasOne(b => b.ApplicationUser)
                .WithMany(a => a.Bookings)
                .HasForeignKey(s => s.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Seat)
                .WithOne(s => s.Booking)
                .HasForeignKey<Booking>(b => b.SeatID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(b => b.PaymentStatus)
                .HasDefaultValue(PaymentStatus.Pending);
            builder.Property(b => b.BookingDate)
                .HasDefaultValue(DateTime.UtcNow);
            

        }
    }
}
