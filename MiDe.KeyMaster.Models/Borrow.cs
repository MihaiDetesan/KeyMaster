using System;

namespace MiDe.KeyMaster.Models
{
    public class Borrow
    {
        public string  KeyId { get; set; }
        public string PersonId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
