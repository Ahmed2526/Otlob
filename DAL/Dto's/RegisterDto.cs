using DAL.Consts;
using System.ComponentModel.DataAnnotations;

namespace DAL.Dto_s
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(100)]
        [RegularExpression(RegexPatterns.EnglishLettersOnlyWithSpacePattern, ErrorMessage = Errors.EnglishLettersOnly)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [RegularExpression(RegexPatterns.EnglishLettersOnlyWithSpacePattern, ErrorMessage = Errors.EnglishLettersOnly)]
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [RegularExpression(RegexPatterns.EgyPhonePattern, ErrorMessage = Errors.Phone)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        [RegularExpression(RegexPatterns.EmailPattern, ErrorMessage = Errors.Email)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, ErrorMessage = Errors.PasswordMinValue, MinimumLength = 6)]
        [RegularExpression(RegexPatterns.PasswordPattern, ErrorMessage = Errors.PasswordPattern)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(250)]
        public string Street { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        [RegularExpression(RegexPatterns.EgyPostalCode, ErrorMessage = Errors.InvalidPostalNumber)]
        public string ZipCode { get; set; } = null!;

    }
}
