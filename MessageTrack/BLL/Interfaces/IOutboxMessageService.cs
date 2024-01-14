using MessageTrack.BLL.DTOs;

namespace MessageTrack.BLL.Interfaces
{
    public interface IOutboxMessageService
    {
        Task<IEnumerable<OutboxMessageDto>> GetOutboxMessages();
    }
}
