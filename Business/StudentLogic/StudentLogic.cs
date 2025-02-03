using AutoMapper;
using DataTransferObjects;
using Entities;
using Repository.Repository.StudentRepository;

namespace Business.StudentLogic
{
    public class StudentLogic : IStudentLogic
    {
        private readonly IMapper _mapper;

        private readonly IStudentRepository _studentRepository;


        public StudentLogic(IMapper mapper, IStudentRepository studentRepository)
        {
            this._mapper = mapper;
            this._studentRepository = studentRepository;
        }
        
        public async Task<StudentDto> FindAsync(int id)
        {
            var entity = await this._studentRepository.FindAsync(id);

            var result = _mapper.Map<StudentDto>(entity);

            return result;
        }

        public async Task<IEnumerable<StudentDto>> GetAllAsync()
        {
            var entities = await this._studentRepository.GetAllAsync();

            var result = _mapper.Map<List<StudentDto>>(entities);

            return result;
        }

        public async Task<IEnumerable<StudentDto>> GetAllByCourseId(int courseId)
        {
            var entities = await this._studentRepository.GetAllByCourseId(courseId);

            var result = _mapper.Map<List<StudentDto>>(entities);

            return result;
        }

        public async Task<CreditsStudentDto> GetCreditsByStudent(int studentId)
        {
            var entity = await this._studentRepository.GetCreditsByStudent(studentId);

            var result = _mapper.Map<CreditsStudentDto>(entity);

            return result;
        }

        public async Task<StudentDto> RemoveAsync(int id)
        {
            var entity = await this._studentRepository.RemoveAsync(id);

            var result = _mapper.Map<StudentDto>(entity);

            return result;
        }

        public async Task<StudentDto> UpdateAsync(int id, StudentDto studentDto)
        {
            var entity = await this._studentRepository.UpdateAsync(id, _mapper.Map<Student>(studentDto));

            var result = _mapper.Map<StudentDto>(entity);

            return result;
        }        

    }
}
