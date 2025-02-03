using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects
{
    public class RegisteredCourseDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Id del estudiante es requerido")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "El Id del curso es requerido")]
        public int CourseId { get; set; }
    }
}
