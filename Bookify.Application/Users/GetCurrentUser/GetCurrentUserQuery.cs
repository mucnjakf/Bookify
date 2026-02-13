using System.Data;
using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Users;
using Dapper;

namespace Bookify.Application.Users.GetCurrentUser;

public sealed record GetCurrentUserQuery : IQuery<UserDto>;

internal sealed class GetCurrentUserQueryHandler(
    ISqlConnectionFactory sqlConnectionFactory,
    IUserContext userContext)
    : IQueryHandler<GetCurrentUserQuery, UserDto>
{
    public async Task<Result<UserDto>> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
    {
        using IDbConnection connection = sqlConnectionFactory.CreateConnection();

        const string sql = """
                            SELECT
                                id as Id,
                                first_name as FirstName,
                                last_name as LastName,
                                email AS Email
                           FROM users
                           WHERE identity_id = @IdentityId
                           """;

        var user = await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new { userContext.IdentityId });

        return user is null ? Result.Failure<UserDto>(UserErrors.NotFound) : Result.Success(user);
    }
}