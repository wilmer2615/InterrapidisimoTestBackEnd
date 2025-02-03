using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects
{
    public class AccountDto
    {

        [Required(ErrorMessage = "El correo del estudiante es requerido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El password del estudiante es requerido")]
        public string Password { get; set; } = null!;
    }
}
