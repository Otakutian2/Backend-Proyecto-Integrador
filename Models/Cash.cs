using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models
{
    public class Cash
    {
        public int Id { get; set; }

        [Column("establishment_id")]
        public int EstablishmentId { get; set; }
        public Establishment Establishment { get; set; }

        public List<Receipt> ReceiptCollection { get; } = new List<Receipt>();
    }
}
