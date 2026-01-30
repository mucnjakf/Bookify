using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews;
using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.EfCore.EntityTypeConfiguration;

internal sealed class ReviewTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");

        builder.HasKey(review => review.Id);

        builder
            .Property(review => review.Rating)
            .HasConversion(rating => rating.Value, value => Rating.Create(value).Value);

        builder
            .Property(review => review.Comment)
            .HasMaxLength(200);

        builder
            .HasOne<Apartment>()
            .WithMany()
            .HasForeignKey(review => review.ApartmentId);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(review => review.UserId);

        builder
            .HasOne<Booking>()
            .WithMany()
            .HasForeignKey(review => review.BookingId);
    }
}