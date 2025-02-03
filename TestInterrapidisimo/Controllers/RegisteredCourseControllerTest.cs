using Business.RegisteredCourseLogic;
using DataTransferObjects;
using Interrapidisimo.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestInterrapidisimo.Controllers
{
    public class RegisteredCourseControllerTest
    {

        private readonly Mock<IRegisteredCorseLogic> _mockRegisteredCorseLogic;
        private readonly RegisteredCourseController _controller;
        public RegisteredCourseControllerTest()
        {
            _mockRegisteredCorseLogic = new Mock<IRegisteredCorseLogic>();
            _controller = new RegisteredCourseController(_mockRegisteredCorseLogic.Object);
        }

        [Fact]
        public async Task Add_ReturnsOk_WhenValidRequest()
        {
            // Arrange
            var newCourse = new RegisteredCourseDto
            {
                Id = 1,
                StudentId = 1,
                CourseId = 5
            };

            _mockRegisteredCorseLogic.Setup(x => x.AddAsync(It.IsAny<RegisteredCourseDto>()))
                                      .ReturnsAsync(newCourse);

            // Act
            var result = await _controller.Add(newCourse);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<RegisteredCourseDto>(okResult.Value);
            Assert.Equal(newCourse.Id, returnValue.Id);
            Assert.Equal(newCourse.StudentId, returnValue.StudentId);
            Assert.Equal(newCourse.CourseId, returnValue.CourseId);
        }

        [Fact]
        public async Task Add_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var invalidCourse = new RegisteredCourseDto();

            _controller.ModelState.AddModelError("StudentId", "El Id del estudiante es requerido");
            _controller.ModelState.AddModelError("CourseId", "El Id del curso es requerido");

            // Act
            var result = await _controller.Add(invalidCourse);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);

            Assert.True(modelState.ContainsKey("StudentId"));
            Assert.True(modelState.ContainsKey("CourseId"));
        }

        [Fact]
        public async Task Remove_ReturnsOk_WhenRecordIsDeleted()
        {
            // Arrange
            int studentId = 10;
            int courseId = 5;
            var deletedCourse = new RegisteredCourseDto { Id = 1, StudentId = studentId, CourseId = courseId };

            _mockRegisteredCorseLogic.Setup(x => x.RemoveAsync(studentId, courseId))
                                      .ReturnsAsync(deletedCourse);

            // Act
            var result = await _controller.Remove(studentId, courseId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Remove_ReturnsBadRequest_WhenInvalidIdsProvided()
        {
            // Arrange
            int studentId = 0;
            int courseId = -1;

            // Act
            var result = await _controller.Remove(studentId, courseId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Remove_ReturnsNotFound_WhenRecordDoesNotExist()
        {
            // Arrange
            int studentId = 15;
            int courseId = 7;

            _mockRegisteredCorseLogic.Setup(x => x.RemoveAsync(studentId, courseId))
                                      .ReturnsAsync((RegisteredCourseDto?)null);

            // Act
            var result = await _controller.Remove(studentId, courseId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("El registro no se encuentra en la base de datos!", notFoundResult?.Value?.ToString());
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithData()
        {
            // Arrange
            var newRegister = new List<RegisteredCourseDto>
        {
            new RegisteredCourseDto { Id = 1, StudentId = 1, CourseId = 2 },
            new RegisteredCourseDto { Id = 2, StudentId = 2, CourseId = 3 }
        };

            _mockRegisteredCorseLogic.Setup(x => x.GetAllAsync()).ReturnsAsync(newRegister);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedData = Assert.IsType<List<RegisteredCourseDto>>(okResult.Value);
            Assert.Equal(2, returnedData.Count);
        }

        [Fact]
        public async Task GetAllById_ValidId_ShouldReturnOk()
        {
            // Arrange
            var newCourses = new List<CoursesResultDto>
            {
                new CoursesResultDto
                {
                    CourseId = 1,
                    CourseName = "Matemáticas",
                    Credits = 5,
                    TeacherId = 10,
                    TeacherName = "Juan",
                    StudentsByCourse = 30
                }
            };

            _mockRegisteredCorseLogic.Setup(x => x.FindAsync(1)).ReturnsAsync(newCourses);

            // Act
            var result = await _controller.GetAllById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedData = Assert.IsType<List<CoursesResultDto>>(okResult.Value);
            Assert.Equal(newCourses[0].CourseId, returnedData[0].CourseId);
            Assert.Equal(newCourses[0].CourseName, returnedData[0].CourseName);
        }

        [Fact]
        public async Task GetAllById_InvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _mockRegisteredCorseLogic?.Setup(x => x.FindAsync(It.IsAny<int>()))
                .ReturnsAsync((List<CoursesResultDto>?)null);

            // Act
            var result = await _controller.GetAllById(99);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("El registro no se encuentra en la base de datos!", notFoundResult?.Value?.ToString());
        }
    }
}
