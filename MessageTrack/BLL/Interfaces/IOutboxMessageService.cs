using MessageTrack.BLL.DTOs;

namespace MessageTrack.BLL.Interfaces
{
    public interface IOutboxMessageService
    {
        Task<IEnumerable<OutboxMessageDto>> GetAllMessagesByExternalRecipientId(int externalRecipientId);
        Task SaveOutboxMessage(OutboxMessageDto outboxMessageDto);
        Task<string> GenerateRegNumber();
        Task<IEnumerable<OutboxMessageDto>> GetOutboxMessages();
        Task DeleteOutboxMessageById(int id);
    }
}
