using Entities;

namespace Repository.Repository.StudentRepository
{
    public interface IStudentRepository
    {
        public Task<Student?> UpdateAsync(int id, Student student);

        public Task<Student?> RemoveAsync(int id);

        public Task<Student?> FindAsync(int id);

        public Task<CreditsStudent?> GetCreditsByStudent(int studentId);

        public Task<IEnumerable<Student>> GetAllByCourseId(int courseId);

        public Task<IEnumerable<Student>> GetAllAsync();
    }
}
