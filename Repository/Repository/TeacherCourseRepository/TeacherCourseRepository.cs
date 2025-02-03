using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository.TeacherCourseRepository
{
    public class TeacherCourseRepository : ITeacherCourseRepository
    {
        private readonly AplicationDbContext _context;

        public TeacherCourseRepository(AplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<CoursesResult>> GetCourseByTeacher(int studentId)
        {

            var teachersByStudent = await (from st in _context.Students
                                           where st.Id == studentId
                                           join rc in _context.RegisteredCourses on st.Id equals rc.StudentId
                                           join c in _context.Courses on rc.CourseId equals c.Id
                                           join tc in _context.TeacherCourses on c.Id equals tc.CourseId
                                           join t in _context.Teachers on tc.TeacherId equals t.Id
                                           select t.Id
                               ).ToListAsync();

            var result = await (from ca in _context.TeacherCourses
                                join c in _context.Courses on ca.CourseId equals c.Id
                                join p in _context.Teachers on ca.TeacherId equals p.Id
                                where !teachersByStudent.Contains(p.Id)
                                select new CoursesResult
                                {
                                    CourseId = c.Id,
                                    CourseName = c.Name,
                                    Credits = c.Credits,
                                    TeacherId = p.Id,
                                    TeacherName = p.Name,
                                    StudentsByCourse = (from rc in _context.RegisteredCourses
                                                        where rc.CourseId == c.Id
                                                        select rc.StudentId).Count()
                                }).ToListAsync();



            return result;
        }
    }
}
