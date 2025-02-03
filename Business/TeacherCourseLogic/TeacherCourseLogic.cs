using AutoMapper;
using DataTransferObjects;
using Repository.Repository.TeacherCourseRepository;

namespace Business.TeacherCourseLogic
{
    public class TeacherCourseLogic : ITeacherCourseLogic
    {
        private readonly IMapper _mapper;

        private readonly ITeacherCourseRepository _teacherCourseRepository;


        public TeacherCourseLogic(IMapper mapper, ITeacherCourseRepository teacherCourseRepository)
        {
            this._mapper = mapper;
            this._teacherCourseRepository = teacherCourseRepository;
        }
        public async Task<IEnumerable<CoursesResultDto>> GetCourseByTeacher(int studenId)
        {
            var entities = await this._teacherCourseRepository.GetCourseByTeacher(studenId);

            var result = _mapper.Map<List<CoursesResultDto>>(entities);

            return result;
        }

    }
}
