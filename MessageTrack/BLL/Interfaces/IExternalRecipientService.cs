using MessageTrack.BLL.DTOs;
using MessageTrack.DAL.Entities;

namespace MessageTrack.BLL.Interfaces
{
    public interface IExternalRecipientService
    {
        Task<ExternalRecipientDto> GetExternalRecipientById(int id);
        Task<IEnumerable<ExternalRecipientDto>> GetExternalRecipients();

        Task<int?> CreateExternalRecipient(ExternalRecipientDto externalRecipientDto);

        Task<bool> CheckUniqueExternalRecipient(string name);
        Task<ExternalRecipientDto> GetExternalRecipientByName(string name);
    }
}
