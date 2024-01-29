using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyLocker.Data.Schema
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(100000, 999999, ErrorMessage = "RequestId must be a 6-digit number between 100000 and 999999")]
        public int RequestId { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public string Status { get; set; }

        public long TransactionNumber { get; set; }

        [Required]
        [MaxLength]
        public string? GatewayResponse { get; set; }

        [Required]
        [MaxLength]
        public string? OrderId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

    }
}
