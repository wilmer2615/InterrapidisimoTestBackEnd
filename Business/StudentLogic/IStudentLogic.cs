using DataTransferObjects;

namespace Business.StudentLogic
{
    public interface IStudentLogic
    {
        public Task<StudentDto> UpdateAsync(int id, StudentDto studentDto);

        public Task<StudentDto> RemoveAsync(int id);

        public Task<IEnumerable<StudentDto>> GetAllAsync();

        public Task<StudentDto> FindAsync(int id);

        public Task<CreditsStudentDto> GetCreditsByStudent(int studentId);

        public Task<IEnumerable<StudentDto>> GetAllByCourseId(int courseId);
    }
}
