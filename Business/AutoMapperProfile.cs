using AutoMapper;
using DataTransferObjects;
using Entities;

namespace Business
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            /// *************************************************** 
            /// Configuracion de mapeos modelo y entidad Student
            /// *************************************************** 
            CreateMap<StudentDto, Student>()
                .ReverseMap();

            /// *************************************************** 
            /// Configuracion de mapeos modelo y entidad RegisteredCourse
            /// *************************************************** 
            CreateMap<RegisteredCourseDto, RegisteredCourse>()
                .ReverseMap();

            /// *************************************************** 
            /// Configuracion de mapeos modelo y entidad TeacherCourse
            /// *************************************************** 
            CreateMap<CoursesResultDto, CoursesResult>()
                .ReverseMap();

            /// *************************************************** 
            /// Configuracion de mapeos modelo y entidad CreditsStudent
            /// *************************************************** 
            CreateMap<CreditsStudentDto, CreditsStudent>()
                .ReverseMap();

        }
    }
}
