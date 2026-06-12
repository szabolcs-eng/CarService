using System.ComponentModel.DataAnnotations;

namespace CarServiceApi.DTOs
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "A felhasználónév megadása kötelező!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "A felhasználónévnek 3 és 50 karakter között kell lennie.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Az e-mail cím megadása kötelező!")]
        [EmailAddress(ErrorMessage = "Érvénytelen e-mail formátum!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A jelszó megadása kötelező!")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A jelszónak legalább 6 karakter hosszúnak kell lennie.")]
        public string Password { get; set; } = string.Empty;
    }
}
