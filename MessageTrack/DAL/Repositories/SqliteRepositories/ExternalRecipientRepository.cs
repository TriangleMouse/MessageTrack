using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MessageTrack.DAL.Entities;
using MessageTrack.DAL.Interfaces.Repositories;
using MessageTrack.DAL.Repositories.Base;

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
    }
}
