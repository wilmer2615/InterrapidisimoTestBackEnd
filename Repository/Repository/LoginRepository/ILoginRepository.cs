using Entities;

namespace Repository.Repository.LoginRepository
{
    public interface ILoginRepository
    {

        public Task<Student> AddAsync(Student student);

        public Task<Student?> VerifyAccountAsync(string email, string password);
    }
}
