using MessageTrack.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageTrack.DAL.Interfaces.Repositories
{
    public interface IOutboxMessageRepository
    {
        Task<IEnumerable<OutboxMessage>> GetOutboxMessages();
    }
}
