using Bookify.Application.Apartments.SearchApartments;
using Bookify.Application.IntegrationTests.Infrastructure;
using Bookify.Domain.Abstractions;
using Shouldly;

namespace Bookify.Application.IntegrationTests.Apartments;

public sealed class SearchApartmentsTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task SearchApartments_Should_ReturnEmptyList_When_DateRangeIsInvalid()
    {
        var query = new SearchApartmentsQuery(
            new DateOnly(2024, 1, 10),
            new DateOnly(2024, 1, 1));

        Result<IReadOnlyList<ApartmentDto>> result = await Sender.Send(query);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeEmpty();
    }

    [Fact]
    public async Task SearchApartments_Should_ReturnApartments_When_DateRangeIsValid()
    {
        var query = new SearchApartmentsQuery(
            new DateOnly(2024, 1, 1),
            new DateOnly(2024, 1, 10));

        Result<IReadOnlyList<ApartmentDto>> result = await Sender.Send(query);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeEmpty();
    }
}