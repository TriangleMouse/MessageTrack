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
                outboxMessages = await Connection.QueryAsync<OutboxMessage>("select Id, Date_Created, Reg_Number, External_Recipient_Id, Notes from Outbox_Message", transaction: Transaction);
            });

            return outboxMessages;
        }
    }
}
