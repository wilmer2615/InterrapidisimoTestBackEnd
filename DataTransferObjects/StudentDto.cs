using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects
{
    public class StudentDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del estudiante es requerido")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El correo del estudiante es requerido")]
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "El password del estudiante es requerido")]
        public string Password { get; set; } = null!;
    }
}
