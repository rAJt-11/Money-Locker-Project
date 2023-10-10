using System.ComponentModel.DataAnnotations;

namespace MoneyLocker.Data.Schema
{
    public class UserInfo
    {
        [Required]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed more than 25 characters")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Last Name cannot exceed more than 25 characters")]
        public string LastName { get; set; }

        [Required]
        [Key]
        [RegularExpression("^[6-9][0-9]{9}$", ErrorMessage = "Invalid Phone No. Format")]
        public string Mobile { get; set; }

        [Required]
        [RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]{1}(?=.*\\d)(?=.*[@#$%&*_])(?=.*[a-zA-Z]).{7,}$", ErrorMessage = "Invalid Password Format")]
        public string Password { get; set; }

    }
}
