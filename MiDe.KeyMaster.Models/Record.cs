
using System;

namespace MiDe.KeyMaster.Models
{
    public class Record
    {
        public string  KeyId { get; set; }
        public string PersonId { get; set; }

        public DateTime BorrowDate { get; set; }

        public DateTime ReturnDate { get; set; }
    }
}
