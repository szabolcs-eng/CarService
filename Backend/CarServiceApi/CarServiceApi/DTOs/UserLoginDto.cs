using System.ComponentModel.DataAnnotations;

namespace CarServiceApi.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Az e-mail cím megadása kötelező!")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A jelszó megadása kötelező!")]
        public string Password { get; set; } = string.Empty;
    }
}
