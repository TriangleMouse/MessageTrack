using MessageTrack.DAL.Entities;

namespace MessageTrack.DAL.Interfaces.Repositories
{
    public interface IOutboxMessageRepository
    {
        Task<IEnumerable<OutboxMessage>> GetOutboxMessages();
        Task<int?> CreateOutboxMessage(OutboxMessage message);
        Task UpdateOutboxMessage(OutboxMessage message);
        Task<OutboxMessage> GetLastOutboxMessage();
        Task DeleteOutboxMessageById(int id);
    }
}
