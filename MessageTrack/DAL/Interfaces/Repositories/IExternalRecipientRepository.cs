using MessageTrack.DAL.Entities;

namespace MessageTrack.DAL.Interfaces.Repositories
{
    public interface IExternalRecipientRepository
    {
        Task<IEnumerable<ExternalRecipient>> GetExternalRecipients();
        Task<int?> CreateExternalRecipient(ExternalRecipient externalRecipient);
        Task<ExternalRecipient> GetExternalRecipientByName(string name);
        Task<ExternalRecipient> GetExternalRecipientById(int id);
        Task DeleteExternalRecipientById(int id);
    }
}
