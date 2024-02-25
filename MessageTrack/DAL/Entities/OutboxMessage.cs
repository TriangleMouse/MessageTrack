namespace MessageTrack.DAL.Entities
{
    public class OutboxMessage
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        
        public int? ExternalRecipientId { get; set; }
        public string RegNumber { get; set; }
        public string Notes { get; set; }
    }
}
