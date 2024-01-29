using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyLocker.Data.Schema
{
    public class UserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(1000, 9999, ErrorMessage = "RequestId must be a 4-digit number between 1000 and 9999")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed more than 25 characters")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Last Name cannot exceed more than 25 characters")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[6-9][0-9]{9}$", ErrorMessage = "Invalid Phone No. Format")]
        public long Mobile { get; set; }

        [Required]
        [RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]{1}(?=.*\\d)(?=.*[@#$%&*_])(?=.*[a-zA-Z]).{7,}$", ErrorMessage = "Invalid Password Format")]
        public string Password { get; set; }

    }
}
