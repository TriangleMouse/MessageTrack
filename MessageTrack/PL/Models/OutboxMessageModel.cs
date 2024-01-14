using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageTrack.PL.Models
{
    public class OutboxMessageModel
    {
        public int Id { get; set; }
        public int FakeId { get; set; }
        public DateTime DateCreated { get; set; }
        public int? ExternalRecipientId { get; set; }
        public string NameExternalRecipient { get; set; }
        public string Reg_Number { get; set; }
        public string Notes { get; set; }
    }
}
