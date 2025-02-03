using Entities;

namespace Repository.Repository.TeacherRepository
{
    public interface ITeacherRepository
    {
        public Task<Teacher?> FindAsync(int id);

        public Task<IEnumerable<Teacher>> GetAllAsync();
    }
}
