using AutoMapper;
using Business.TeacherCourseLogic;
using DataTransferObjects;
using Entities;
using Moq;
using Repository.Repository.TeacherCourseRepository;

namespace TestBusiness.TeacherCourseLogic
{
    public class TeacherCourseLogicTest
    {
        private readonly Mock<ITeacherCourseRepository> _mockTeacherCourseRepository;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ITeacherCourseLogic _teacherCourseLogic;

        public TeacherCourseLogicTest()
        {
            _mockTeacherCourseRepository = new Mock<ITeacherCourseRepository>();
            _mapperMock = new Mock<IMapper>();
            _teacherCourseLogic = new Business.TeacherCourseLogic.TeacherCourseLogic(_mapperMock.Object, _mockTeacherCourseRepository.Object);
        }

        [Fact]
        public async Task GetCourseByTeacher_ReturnsMappedCourses()
        {
            // Arrange
            int studentId = 1;

            var mockEntities = new List<CoursesResult>
        {
            new CoursesResult { CourseId = 1, CourseName = "Matematicas", Credits = 3, TeacherId = 1, TeacherName = "Diana", StudentsByCourse = 2 },
            new CoursesResult { CourseId = 2, CourseName = "Fisica", Credits = 3, TeacherId = 2, TeacherName = "Jhon", StudentsByCourse = 3 }
        };

            var mockCoursesDto = new List<CoursesResultDto>
        {
            new CoursesResultDto { CourseId = 1, CourseName = "Matematicas", Credits = 3, TeacherId = 1, TeacherName = "Diana", StudentsByCourse = 2 },
            new CoursesResultDto { CourseId = 2, CourseName = "Fisica", Credits = 3, TeacherId = 2, TeacherName = "Jhon", StudentsByCourse = 3 }
        };

            _mockTeacherCourseRepository.Setup(x => x.GetCourseByTeacher(studentId))
                                        .ReturnsAsync(mockEntities);

            _mapperMock.Setup(m => m.Map<List<CoursesResultDto>>(mockEntities))
                       .Returns(mockCoursesDto);

            // Act
            var result = await _teacherCourseLogic.GetCourseByTeacher(studentId);

            // Assert
            Assert.NotNull(result);
            var resultList = Assert.IsType<List<CoursesResultDto>>(result);
            Assert.Equal(2, resultList.Count);
            Assert.Equal("Matematicas", resultList[0].CourseName);
            Assert.Equal("Fisica", resultList[1].CourseName);
        }

        [Fact]
        public async Task GetCourseByTeacher_ReturnsEmptyList_WhenNoCoursesFound()
        {
            // Arrange
            int studentId = 2;

            _mockTeacherCourseRepository.Setup(x => x.GetCourseByTeacher(studentId))
                                        .ReturnsAsync(new List<CoursesResult>()); // Devuelve lista vacía

            _mapperMock.Setup(m => m.Map<List<CoursesResultDto>>(It.IsAny<List<CoursesResult>>()))
                       .Returns(new List<CoursesResultDto>());

            // Act
            var result = await _teacherCourseLogic.GetCourseByTeacher(studentId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
