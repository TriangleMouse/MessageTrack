using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageTrack.DAL.Interfaces.Repositories;

namespace MessageTrack.DAL.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IExternalRecipientRepository ExternalRecipientRepository { get; }
        IOutboxMessageRepository OutboxMessageRepository { get; }

        void Commit();
        void Rollback();
    }
}
