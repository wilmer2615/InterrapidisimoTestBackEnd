using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository.LoginRepository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly AplicationDbContext _context;

        public LoginRepository(AplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<Student> AddAsync(Student student)
        {
            await this._context.Set<Student>().AddAsync(student);
            await this._context.SaveChangesAsync();

            await this._context.Set<CreditsStudent>().AddAsync(new CreditsStudent { Total = 9, StudentId = student.Id });
            await this._context.SaveChangesAsync();

            return student;
        }

        public async Task<Student?> VerifyAccountAsync(string email, string password)
        {
            var entity = await _context.Set<Student>().FirstOrDefaultAsync(x => x.Email == email && x.Password == password);

            return entity;
        }
    }
}
