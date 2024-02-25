namespace MessageTrack.PL.Models
{
    public class OutboxMessageModel
    {
        public int Id { get; set; }
        public int FakeId { get; set; }
        public string DateCreated { get; set; } = DateTime.Now.ToString("dd MMMM yyyy");
        public int? ExternalRecipientId { get; set; }
        public string NameExternalRecipient { get; set; }
        public string RegNumber { get; set; } = "*";
        public string Notes { get; set; }
    }
}
