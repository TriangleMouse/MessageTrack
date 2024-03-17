using Dapper;
using MessageTrack.DAL.Entities;
using MessageTrack.DAL.Interfaces.Repositories;
using MessageTrack.DAL.Repositories.Base;
using System.Data;

namespace MessageTrack.DAL.Repositories.SqliteRepositories
{
    public class OutboxMessageRepository : RepositoryBase<OutboxMessage>, IOutboxMessageRepository
    {
        public OutboxMessageRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public async Task<IEnumerable<OutboxMessage>> GetOutboxMessages()
        {
            var outboxMessages = Enumerable.Empty<OutboxMessage>();

            await SafeExecuteAsync(async () =>
            {
                outboxMessages = await Connection.QueryAsync<OutboxMessage>("select Id, DateCreated, RegNumber, ExternalRecipientId, Notes from Outbox_Message", transaction: Transaction);
            });

            return outboxMessages;
        }


        public async Task<IEnumerable<OutboxMessage>> GetAllMessagesByExternalRecipientId(int externalRecipientId)
        {
            var outboxMessages = Enumerable.Empty<OutboxMessage>();

            await SafeExecuteAsync(async () =>
            {
                outboxMessages = await Connection.QueryAsync<OutboxMessage>("select Id, DateCreated, RegNumber, ExternalRecipientId, Notes from Outbox_Message  where ExternalRecipientId = @externalRecipientId", new { externalRecipientId = externalRecipientId }, transaction: Transaction);
            });

            return outboxMessages;
        }

        public async Task<OutboxMessage> GetLastOutboxMessage()
        {
            OutboxMessage message = default;

            await SafeExecuteAsync(async () =>
            {
                message = await Connection.QueryFirstOrDefaultAsync<OutboxMessage>(
                    "SELECT Id, DateCreated, RegNumber, ExternalRecipientId, Notes FROM Outbox_Message ORDER BY id DESC LIMIT 1");
            });

            return message;
        }

        public async Task<int?> CreateOutboxMessage(OutboxMessage message)
        {
            int? id = default;

            await SafeExecuteAsync(async () =>
            {
                id = await Connection.ExecuteScalarAsync<int?>("insert into Outbox_Message (DateCreated, RegNumber, ExternalRecipientId, Notes) values(@DateCreated,  @RegNumber, @ExternalRecipientId, @Notes) RETURNING Id", new { DateCreated = message.DateCreated, RegNumber = message.RegNumber, ExternalRecipientId = message.ExternalRecipientId, Notes = message.Notes }, transaction: Transaction);
            });

            return id;
        }

        public async Task DeleteOutboxMessageById(int id)
        {
            await SafeExecuteAsync(async () =>
            {
                await Connection.ExecuteAsync("delete from Outbox_Message where id = @id", new { id = id }, transaction: Transaction);
            });
        }

        public async Task UpdateOutboxMessage(OutboxMessage message)
        {
            await SafeExecuteAsync(async () =>
            {
                await Connection.ExecuteAsync("update Outbox_Message set ExternalRecipientId = @ExternalRecipientId, Notes = @Notes where id = @id", new { ExternalRecipientId = message.ExternalRecipientId, Notes = message.Notes, id = message.Id }, transaction: Transaction);
            });
        }
    }
}
