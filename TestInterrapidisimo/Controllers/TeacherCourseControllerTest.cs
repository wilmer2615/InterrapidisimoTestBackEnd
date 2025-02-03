using Business.TeacherCourseLogic;
using DataTransferObjects;
using Interrapidisimo.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestInterrapidisimo.Controllers
{
    public class TeacherCourseControllerTest
    {
        private readonly Mock<ITeacherCourseLogic> _mockTeacherCourseLogic;
        private readonly TeacherCourseController _controller;
        public TeacherCourseControllerTest()
        {
            _mockTeacherCourseLogic = new Mock<ITeacherCourseLogic>();
            _controller = new TeacherCourseController(_mockTeacherCourseLogic.Object);
        }

        [Fact]
        public async Task GetCourseByTeacher_ReturnsOk_WithCourseList()
        {
            // Arrange
            int studentId = 1;
            var mockCourses = new List<CoursesResultDto>
        {
            new CoursesResultDto { CourseId = 1, CourseName = "Matematicas", Credits = 3, TeacherId = 1, TeacherName = "Diana", StudentsByCourse = 2 },
            new CoursesResultDto { CourseId = 2, CourseName = "Fisica", Credits = 3, TeacherId = 2, TeacherName = "Jhon", StudentsByCourse = 3 }
        };

            _mockTeacherCourseLogic.Setup(x => x.GetCourseByTeacher(studentId))
                                   .ReturnsAsync(mockCourses);

            // Act
            var result = await _controller.GetCourseByTeacher(studentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCourses = Assert.IsType<List<CoursesResultDto>>(okResult.Value);

            Assert.Equal(2, returnedCourses.Count);
            Assert.Equal("Matematicas", returnedCourses[0].CourseName);
            Assert.Equal("Fisica", returnedCourses[1].CourseName);
        }

        [Fact]
        public async Task GetCourseByTeacher_ReturnsOk_WithEmptyList_WhenNoCoursesFound()
        {
            // Arrange
            int studentId = 2;
            _mockTeacherCourseLogic.Setup(x => x.GetCourseByTeacher(studentId))
                                   .ReturnsAsync(new List<CoursesResultDto>());

            // Act
            var result = await _controller.GetCourseByTeacher(studentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCourses = Assert.IsType<List<CoursesResultDto>>(okResult.Value);

            Assert.Empty(returnedCourses);
        }
    }
}
