using System.Data;
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Dapper;

namespace Bookify.Application.Apartments.SearchApartments;

internal sealed record SearchApartmentsQuery(DateOnly StartDate, DateOnly EndDate)
    : IQuery<IReadOnlyList<ApartmentResponse>>;

internal sealed class SearchApartmentsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<ApartmentResponse>>
{
    private static readonly int[] ActiveBookingStatuses =
    [
        (int)BookingStatus.Reserved,
        (int)BookingStatus.Confirmed,
        (int)BookingStatus.Completed
    ];

    public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(
        SearchApartmentsQuery query,
        CancellationToken cancellationToken)
    {
        if (query.StartDate > query.EndDate)
        {
            return Result.Success<IReadOnlyList<ApartmentResponse>>([]);
        }

        IDbConnection dbConnection = sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                               a.id AS Id,
                               a.name AS Name,
                               a.description AS Description,
                               a.price_amount AS Price,
                               a.price_currency AS Currency,
                               a.address_country AS Country,
                               a.address_state AS State,
                               a.address_zip_code AS ZipCode,
                               a.address_city AS City,
                               a.address_street AS Street
                           FROM apartments a
                           WHERE NOT EXISTS
                           (
                               SELECT 1
                               FROM bookings b
                               WHERE
                                   b.apartment_id = a.id AND
                                   b.duration_start <= @EndDate AND
                                   b.duration_end >= @StartDate AND
                                   b.status = ANY(@ActiveBookingStatuses)
                                   
                           )
                           """;

        IEnumerable<ApartmentResponse> apartments = await dbConnection
            .QueryAsync<ApartmentResponse, AddressResponse, ApartmentResponse>(
                sql,
                (apartment, address) =>
                {
                    apartment.Address = address;
                    return apartment;
                },
                new
                {
                    query.StartDate,
                    query.EndDate,
                    ActiveBookingStatuses
                },
                splitOn: "Country");

        return Result.Success<IReadOnlyList<ApartmentResponse>>(apartments.ToList());
    }
}