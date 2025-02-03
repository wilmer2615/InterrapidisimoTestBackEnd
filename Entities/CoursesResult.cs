namespace Entities
{
    public class CoursesResult
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = null!;
        public int Credits { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = null!;
        public int StudentsByCourse { get; set; }
    }
}
