using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository.TeacherRepository
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AplicationDbContext _context;

        public TeacherRepository(AplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<Teacher?> FindAsync(int id)
        {
            return await this._context.Set<Teacher>()
               .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await this._context.Set<Teacher>()
                .ToListAsync();
        }
    }
}
