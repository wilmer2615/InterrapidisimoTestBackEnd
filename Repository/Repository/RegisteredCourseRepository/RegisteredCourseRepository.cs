using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository.RegisteredCourseRepository
{
    public class RegisteredCourseRepository : IRegisteredCourseRepository
    {
        private readonly AplicationDbContext _context;

        public RegisteredCourseRepository(AplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<RegisteredCourse> AddAsync(RegisteredCourse registeredCourse)
        {
            await this._context.Set<RegisteredCourse>().AddAsync(registeredCourse);
            await this._context.SaveChangesAsync();

            return registeredCourse;
        }

        public async Task<List<CoursesResult>> FindAsync(int id)
        {

            var result = await (from reg in _context.RegisteredCourses
                                join c in _context.Courses on reg.CourseId equals c.Id
                                join tc in _context.TeacherCourses on c.Id equals tc.CourseId
                                join t in _context.Teachers on tc.TeacherId equals t.Id
                                where reg.StudentId == id
                                select new CoursesResult
                                {
                                    CourseId = c.Id,
                                    CourseName = c.Name,
                                    Credits = c.Credits,
                                    TeacherId = t.Id,
                                    TeacherName = t.Name

                                }).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<RegisteredCourse>> GetAllAsync()
        {
            return await this._context.Set<RegisteredCourse>()
                .ToListAsync();
        }

        public async Task<RegisteredCourse?> RemoveAsync(int studentId, int courseId)
        {
            var entity = await _context.Set<RegisteredCourse>().FirstOrDefaultAsync(x => x.StudentId == studentId && x.CourseId == courseId);

            if (entity != null)
            {
                var result = this._context.Set<RegisteredCourse>().Remove(entity);
                await this._context.SaveChangesAsync();

                return result.Entity;
            }
            return null;
        }
    }
}
