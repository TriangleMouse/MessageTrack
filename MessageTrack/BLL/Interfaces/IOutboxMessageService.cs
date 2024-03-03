using MessageTrack.BLL.DTOs;

namespace MessageTrack.BLL.Interfaces
{
    public interface IOutboxMessageService
    {
        Task CreateOutboxMessage(OutboxMessageDto outboxMessageDto);
        Task<IEnumerable<OutboxMessageDto>> GetOutboxMessages();
        Task DeleteOutboxMessageById(int id);
        Task UpdateOutboxMessage(OutboxMessageDto outboxMessageDto);
    }
}
