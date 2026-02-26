using System.Data;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Data;
using Bookify.Domain.Abstractions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Bookify.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob(
    ISqlConnectionFactory sqlConnectionFactory,
    IPublisher publisher,
    IDateTimeProvider dateTimeProvider,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxMessagesJob> logger) : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Started processing outbox messages");

        using IDbConnection dbConnection = sqlConnectionFactory.CreateConnection();
        using IDbTransaction transaction = dbConnection.BeginTransaction();

        IReadOnlyList<OutboxMessageDto> outboxMessages = await GetOutboxMessagesAsync(dbConnection, transaction);

        foreach (OutboxMessageDto outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                var domainEvent = JsonConvert
                    .DeserializeObject<IDomainEvent>(outboxMessage.Content, JsonSerializerSettings)!;

                await publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch (Exception caughtException)
            {
                logger.LogError(
                    caughtException,
                    "Exception while processing outbox message {MessageId}",
                    outboxMessage.Id);

                exception = caughtException;
            }

            await UpdateOutboxMessageAsync(dbConnection, transaction, outboxMessage, exception);
        }

        transaction.Commit();

        logger.LogInformation("Completed processing outbox messages");
    }

    private static async Task<IReadOnlyList<OutboxMessageDto>> GetOutboxMessagesAsync(
        IDbConnection dbConnection,
        IDbTransaction transaction)
    {
        const string sql = """
                           SELECT id, content
                           FROM outbox_messages
                           WHERE processed_on_utc IS NULL
                           ORDER BY occurred_on_utc
                           LIMIT {outboxOptions.Value.BatchSize}
                           FOR UPDATE
                           """;

        IEnumerable<OutboxMessageDto> outboxMessages = await dbConnection
            .QueryAsync<OutboxMessageDto>(sql, transaction: transaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessageAsync(
        IDbConnection dbConnection,
        IDbTransaction transaction,
        OutboxMessageDto outboxMessage,
        Exception? exception)
    {
        const string sql = """
                           UPDATE outbox_messages
                           SET processed_on_utc = @ProcessedOnUtc
                               error = @Error
                           WHERE id = @Id
                           """;

        await dbConnection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = dateTimeProvider.UtcNow,
                Error = exception?.ToString()
            },
            transaction);
    }

    internal sealed record OutboxMessageDto(Guid Id, string Content);
}