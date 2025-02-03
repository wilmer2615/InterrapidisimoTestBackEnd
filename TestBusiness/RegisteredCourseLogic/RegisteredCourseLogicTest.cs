using AutoMapper;
using Business.RegisteredCourseLogic;
using DataTransferObjects;
using Entities;
using Moq;
using Repository.Repository.RegisteredCourseRepository;

namespace TestBusiness.RegisteredCourseLogic
{
    public class RegisteredCourseLogicTest
    {

        private readonly Mock<IRegisteredCourseRepository> _mockRegisteredCourseRepository;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IRegisteredCorseLogic _registeredCorseLogic;
        public RegisteredCourseLogicTest()
        {
            _mockRegisteredCourseRepository = new Mock<IRegisteredCourseRepository>();
            _mapperMock = new Mock<IMapper>();
            _registeredCorseLogic = new Business.RegisteredCourseLogic.RegisteredCourseLogic(_mapperMock.Object, _mockRegisteredCourseRepository.Object);
        }


        [Fact]
        public async Task AddAsync_ValidDto_ReturnsRegisteredCourseDto()
        {
            // Arrange
            var inputDto = new RegisteredCourseDto
            {
                Id = 1,
                StudentId = 100,
                CourseId = 200
            };

            var entity = new RegisteredCourse
            {
                Id = 1,
                StudentId = 100,
                CourseId = 200,
                Student = new Student
                {
                    Id = 100,
                    Name = "Juan Pérez",
                    Email = "juan@example.com",
                    Phone = "123456789",
                    Password = "hashedpassword"
                },
                Course = new Course
                {
                    Id = 200,
                    Name = "Matemáticas",
                    Credits = 5
                }
            };

            var expectedDto = new RegisteredCourseDto
            {
                Id = 1,
                StudentId = 100,
                CourseId = 200
            };

            // Mapeo del DTO a la entidad
            _mapperMock
                .Setup(m => m.Map<RegisteredCourse>(inputDto))
                .Returns(entity);

            // Simula la respuesta del repositorio
            _mockRegisteredCourseRepository.Setup(repo => repo.AddAsync(It.IsAny<RegisteredCourse>()))
                .ReturnsAsync(entity);

            // Mapeo de la entidad devuelta al DTO
            _mapperMock
                .Setup(m => m.Map<RegisteredCourseDto>(entity))
                .Returns(expectedDto);

            // Act
            var result = await _registeredCorseLogic.AddAsync(inputDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.StudentId, result.StudentId);
            Assert.Equal(expectedDto.CourseId, result.CourseId);
        }

        [Fact]
        public async Task FindAsync_ValidId_ReturnsCoursesResultDtoList()
        {
            // Arrange
            var registeredCourseId = 1;
            var courseResults = new List<CoursesResult>
            {
                new CoursesResult
                {
                    CourseId = 101,
                    CourseName = "Matemáticas",
                    Credits = 5,
                    TeacherId = 201,
                    TeacherName = "Profesor A",
                    StudentsByCourse = 30
                },
                new CoursesResult
                {
                    CourseId = 102,
                    CourseName = "Historia",
                    Credits = 4,
                    TeacherId = 202,
                    TeacherName = "Profesor B",
                    StudentsByCourse = 25
                }
            };

            var expectedDtoList = new List<CoursesResultDto>
            {
                new CoursesResultDto
                {
                    CourseId = 101,
                    CourseName = "Matemáticas",
                    Credits = 5,
                    TeacherId = 201,
                    TeacherName = "Profesor A",
                    StudentsByCourse = 30
                },
                new CoursesResultDto
                {
                    CourseId = 102,
                    CourseName = "Historia",
                    Credits = 4,
                    TeacherId = 202,
                    TeacherName = "Profesor B",
                    StudentsByCourse = 25
                }
            };

            // Mapeo de la entidad a DTO
            _mapperMock
                .Setup(m => m.Map<List<CoursesResultDto>>(It.IsAny<List<CoursesResult>>()))
                .Returns(expectedDtoList);

            // llamada al repositorio para obtener los registros
            _mockRegisteredCourseRepository
                .Setup(repo => repo.FindAsync(registeredCourseId))
                .ReturnsAsync(courseResults);

            // Act
            var result = await _registeredCorseLogic.FindAsync(registeredCourseId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<CoursesResultDto>>(result);
            Assert.Equal(expectedDtoList.Count, result.Count);
            Assert.Equal(expectedDtoList[0].CourseId, result[0].CourseId);
            Assert.Equal(expectedDtoList[0].CourseName, result[0].CourseName);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsRegisteredCourseDtoList()
        {
            // Arrange
            var registeredCourses = new List<RegisteredCourse>
            {
                new RegisteredCourse
                {
                    Id = 1,
                    StudentId = 101,
                    CourseId = 201,
                    Student = new Student { Id = 101, Name = "John", Email = "john@prueba.com" },
                    Course = new Course { Id = 201, Name = "Matematicas", Credits = 3 }
                },
                new RegisteredCourse
                {
                    Id = 2,
                    StudentId = 102,
                    CourseId = 202,
                    Student = new Student { Id = 102, Name = "Jane", Email = "jane@prueba.com" },
                    Course = new Course { Id = 202, Name = "Historia", Credits = 4 }
                }
            };

            var expectedDtoList = new List<RegisteredCourseDto>
            {
                new RegisteredCourseDto
                {
                    Id = 1,
                    StudentId = 101,
                    CourseId = 201
                },
                new RegisteredCourseDto
                {
                    Id = 2,
                    StudentId = 102,
                    CourseId = 202
                }
            };

            // Mapeo de la entidad a DTO
            _mapperMock
                .Setup(m => m.Map<List<RegisteredCourseDto>>(It.IsAny<List<RegisteredCourse>>()))
                .Returns(expectedDtoList);

            // llamada al repositorio para obtener los registros
            _mockRegisteredCourseRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(registeredCourses);

            // Act
            var result = await _registeredCorseLogic.GetAllAsync();

            // Assert
            var resultList = result.ToList();  // Convertimos el IEnumerable a List

            Assert.NotNull(resultList);
            Assert.IsType<List<RegisteredCourseDto>>(resultList); 
            Assert.Equal(expectedDtoList.Count, resultList.Count);
            Assert.Equal(expectedDtoList[0].Id, resultList[0].Id); 
            Assert.Equal(expectedDtoList[0].StudentId, resultList[0].StudentId);
            Assert.Equal(expectedDtoList[0].CourseId, resultList[0].CourseId);
        }

        [Fact]
        public async Task RemoveAsync_ReturnsRegisteredCourseDto_WhenEntityIsFound()
        {
            // Arrange
            int studentId = 101;
            int courseId = 202;

            var registeredCourse = new RegisteredCourse
            {
                Id = 1,
                StudentId = studentId,
                CourseId = courseId,
                Student = new Student { Id = studentId, Name = "John", Email = "john@prueba.com" },
                Course = new Course { Id = courseId, Name = "Matematicas", Credits = 3 }
            };

            var expectedDto = new RegisteredCourseDto
            {
                Id = 1,
                StudentId = studentId,
                CourseId = courseId
            };

            // Mapeo de la entidad a DTO
            _mapperMock
                .Setup(m => m.Map<RegisteredCourseDto>(It.IsAny<RegisteredCourse>()))
                .Returns(expectedDto);

            // Llamada del repositorio
            _mockRegisteredCourseRepository
                .Setup(repo => repo.RemoveAsync(studentId, courseId))
                .ReturnsAsync(registeredCourse);

            // Act
            var result = await _registeredCorseLogic.RemoveAsync(studentId, courseId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RegisteredCourseDto>(result); 
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.StudentId, result.StudentId);
            Assert.Equal(expectedDto.CourseId, result.CourseId);
        }

        [Fact]
        public async Task RemoveAsync_ReturnsNull_WhenEntityIsNotFound()
        {
            // Arrange
            int studentId = 101;
            int courseId = 202;

            // Llamada del repositorio 
            _mockRegisteredCourseRepository
                .Setup(repo => repo.RemoveAsync(studentId, courseId))
                .ReturnsAsync((RegisteredCourse?)null);

            // Act
            var result = await _registeredCorseLogic.RemoveAsync(studentId, courseId);

            // Assert
            Assert.Null(result);  // Verifica que el resultado sea nulo
            
        }

    }
}
