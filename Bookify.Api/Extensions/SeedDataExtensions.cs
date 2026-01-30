using System.Data;
using Bogus;
using Bookify.Application.Abstractions.Data;
using Dapper;

namespace Bookify.Api.Extensions;

internal static class SeedDataExtensions
{
    internal static void SeedData(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using IDbConnection dbConnection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker();

        List<object> apartments = [];

        for (var i = 0; i < 100; i++)
        {
            apartments.Add(new
            {
                Id = Guid.NewGuid(),
                Name = faker.Company.CompanyName(),
                Description = faker.Lorem.Sentence(),
                Country = faker.Address.Country(),
                State = faker.Address.State(),
                ZipCode = faker.Address.ZipCode(),
                City = faker.Address.City(),
                Street = faker.Address.StreetAddress(),
                PriceAmount = faker.Random.Decimal(50, 1000),
                PriceCurrency = "USD",
                CleaningFeeAmount = faker.Random.Decimal(25, 200),
                CleaningFeeCurrency = "USD",
                LastBookedOnUtc = DateTime.MinValue,
                Amenities = new List<int> { faker.Random.Int(0, 9) }
            });
        }

        const string sql = """
                           INSERT INTO public.apartments
                           (id, "name", description, address_country, address_state, address_zip_code, address_city, address_street, price_amount, price_currency, cleaning_fee_amount, cleaning_fee_currency, last_booked_on_utc, amenities)
                           VALUES (@Id, @Name, @Description, @Country, @State, @ZipCode, @City, @Street, @PriceAmount, @PriceCurrency, @CleaningFeeAmount, @CleaningFeeCurrency, @LastBookedOnUtc, @Amenities)
                           """;

        dbConnection.Execute(sql, apartments);
    }
}