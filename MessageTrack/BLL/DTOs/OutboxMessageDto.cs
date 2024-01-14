namespace MessageTrack.BLL.DTOs
{
    public class OutboxMessageDto
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int? ExternalRecipientId { get; set; }
        public string Reg_Number { get; set; }
        public string Notes { get; set; }
    }
}
