using Entities;

namespace Repository.Repository.TeacherCourseRepository
{
    public interface ITeacherCourseRepository
    {
        public Task<IEnumerable<CoursesResult>> GetCourseByTeacher(int studentId);
    }
}
