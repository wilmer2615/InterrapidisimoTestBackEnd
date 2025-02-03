using DataTransferObjects;

namespace Business.TeacherCourseLogic
{
    public interface ITeacherCourseLogic
    {
        public Task<IEnumerable<CoursesResultDto>> GetCourseByTeacher(int studentId);
    }
}
