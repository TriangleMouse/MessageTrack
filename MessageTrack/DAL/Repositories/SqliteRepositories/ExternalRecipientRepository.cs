using Dapper;
using MessageTrack.DAL.Entities;
using MessageTrack.DAL.Interfaces.Repositories;
using MessageTrack.DAL.Repositories.Base;
using System.Data;

namespace MessageTrack.DAL.Repositories.SqliteRepositories
{
    public class ExternalRecipientRepository : RepositoryBase<ExternalRecipient>, IExternalRecipientRepository
    {
        public ExternalRecipientRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public async Task<IEnumerable<ExternalRecipient>> GetExternalRecipients()
        {
            var externalRecipients = Enumerable.Empty<ExternalRecipient>();

            await SafeExecuteAsync(async () =>
            {
                externalRecipients = await Connection.QueryAsync<ExternalRecipient>("select Id, Name from External_Recipient", transaction: Transaction);
            });

            return externalRecipients;
        }

        public async Task<int?> CreateExternalRecipient(ExternalRecipient externalRecipient)
        {
            int? id = default;

            await SafeExecuteAsync(async () =>
            {
                id = await Connection.ExecuteScalarAsync<int?>("insert into External_Recipient (Name) values(@Name) RETURNING Id", new{ externalRecipient.Name }, transaction: Transaction);
            });

            return id;
        }

        public async Task<ExternalRecipient> GetExternalRecipientByName(string name)
        {
            ExternalRecipient externalRecipient = default;

            await SafeExecuteAsync(async () =>
            {
                var externalRecipients = await Connection.QueryAsync<ExternalRecipient>("select Id, Name from External_Recipient where Name like @name", new { name = name } , transaction: Transaction);
                externalRecipient = externalRecipients.FirstOrDefault();
            });

            return externalRecipient;
        }

        public async Task<ExternalRecipient> GetExternalRecipientById(int id)
        {
            ExternalRecipient externalRecipient = default;

            await SafeExecuteAsync(async () =>
            {
                externalRecipient = await Connection.QueryFirstOrDefaultAsync<ExternalRecipient>("select Id, Name from External_Recipient where Id = @id", new { id }, transaction: Transaction);
            });

            return externalRecipient;
        }

        public async Task DeleteExternalRecipientById(int id)
        {
            await SafeExecuteAsync(async () =>
            {
                await Connection.ExecuteAsync("delete from External_Recipient where Id = @id", new { id = id }, transaction: Transaction);
            });
        }
    }
}
