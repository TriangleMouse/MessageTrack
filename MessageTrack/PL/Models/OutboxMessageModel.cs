namespace MessageTrack.PL.Models
{
    public class OutboxMessageModel : ICloneable
    {
        public int? Id { get; set; }
        public int FakeId { get; set; }
        public string DateCreated { get; set; } = DateTime.Now.ToString("dd MMMM yyyy");
        public int? ExternalRecipientId { get; set; }
        public string NameExternalRecipient { get; set; }
        public string RegNumber { get; set; } = "*";
        public string Notes { get; set; }

        public object Clone()
        {
            return new OutboxMessageModel
            {
                Id = this.Id,
                FakeId = this.FakeId,
                DateCreated = this.DateCreated,
                ExternalRecipientId = this.ExternalRecipientId,
                NameExternalRecipient = this.NameExternalRecipient,
                RegNumber = this.RegNumber,
                Notes = this.Notes
            };
        }
    }
}
