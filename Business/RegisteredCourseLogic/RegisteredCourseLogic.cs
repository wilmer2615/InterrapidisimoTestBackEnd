using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataTransferObjects;
using Entities;
using Repository.Repository.RegisteredCourseRepository;

namespace Business.RegisteredCourseLogic
{
    public class RegisteredCourseLogic : IRegisteredCorseLogic
    {
        private readonly IMapper _mapper;

        private readonly IRegisteredCourseRepository _registeredCorseRepository;


        public RegisteredCourseLogic(IMapper mapper, IRegisteredCourseRepository registeredCorseRepository)
        {
            this._mapper = mapper;
            this._registeredCorseRepository = registeredCorseRepository;
        }
        public async Task<RegisteredCourseDto> AddAsync(RegisteredCourseDto registeredCourseDto)
        {
            var entity = await this._registeredCorseRepository.AddAsync(_mapper.Map<RegisteredCourse>(registeredCourseDto));

            var result = _mapper.Map<RegisteredCourseDto>(entity);

            return result;
        }

        public async Task<List<CoursesResultDto>> FindAsync(int id)
        {
            var entities = await this._registeredCorseRepository.FindAsync(id);

            var result = _mapper.Map<List<CoursesResultDto>>(entities);

            return result;
        }

        public async Task<IEnumerable<RegisteredCourseDto>> GetAllAsync()
        {
            var entities = await this._registeredCorseRepository.GetAllAsync();

            var result = _mapper.Map<List<RegisteredCourseDto>>(entities);

            return result;
        }

        public async Task<RegisteredCourseDto> RemoveAsync(int studentId, int courseId)
        {
            var entity = await this._registeredCorseRepository.RemoveAsync(studentId, courseId);

            var result = _mapper.Map<RegisteredCourseDto>(entity);

            return result;
        }

    }
}
