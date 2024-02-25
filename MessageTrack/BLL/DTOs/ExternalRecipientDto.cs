namespace MessageTrack.BLL.DTOs
{
    public class ExternalRecipientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ExternalRecipientDto(){}

        public ExternalRecipientDto(string name)
        {
            Name = name;
        }
    }
}
