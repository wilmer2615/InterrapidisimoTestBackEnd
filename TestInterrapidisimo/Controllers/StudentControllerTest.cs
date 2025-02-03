using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.StudentLogic;
using Business.TeacherCourseLogic;
using DataTransferObjects;
using Interrapidisimo.Controllers;
using Interrapidisimo.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace TestInterrapidisimo.Controllers
{
    public class StudentControllerTest
    {
        private readonly Mock<IStudentLogic> _mockStudentLogic;
        private readonly StudentController _controller;

        public StudentControllerTest()
        {
            _mockStudentLogic = new Mock<IStudentLogic>();
            _controller = new StudentController(_mockStudentLogic.Object);
        }        

        [Fact]
        public async Task Update_ReturnsOk_WhenStudentDtoIsValid()
        {
            // Arrange
            int studentId = 1;
            var studentDto = new StudentDto
            {
                Id = studentId,
                Name = "John",
                Email = "john@prueba.com",
                Phone = "9876543210",
                Password = "NewPassword123"
            };

            var updatedStudentDto = new StudentDto
            {
                Id = studentId,
                Name = "John",
                Email = "john@prueba.com",
                Phone = "9876543210",
                Password = "NewPassword123"
            };

            _mockStudentLogic.Setup(logic => logic.UpdateAsync(studentId, studentDto))
                .ReturnsAsync(updatedStudentDto);

            // Act
            var result = await _controller.Update(studentId, studentDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockStudentLogic.Verify(logic => logic.UpdateAsync(studentId, studentDto), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var studentDto = new StudentDto
            {
                Id = 0, // Este id no es válido
                Name = "John",
                Email = "john@prueba.com",
                Phone = "9876543210",
                Password = "NewPassword123"
            };

            _controller.ModelState.AddModelError("Id", "El Id es requerido.");

            // Act
            var result = await _controller.Update(0, studentDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(returnValue.ContainsKey("Id"));
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenStudentNotFound()
        {
            // Arrange
            int studentId = 1;
            var studentDto = new StudentDto
            {
                Id = studentId,
                Name = "John",
                Email = "john@prueba.com",
                Phone = "9876543210",
                Password = "NewPassword123"
            };

            _mockStudentLogic.Setup(logic => logic.UpdateAsync(studentId, studentDto))
                .ReturnsAsync((StudentDto?)null);

            // Act
            var result = await _controller.Update(studentId, studentDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("El estudiante no esta registrado en la base de datos!", notFoundResult?.Value?.ToString());
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenStudentsExist()
        {
            // Arrange
            var studentDtos = new List<StudentDto>
            {
                new StudentDto { Id = 1, Name = "John", Email = "john@prueba.com", Phone = "1234567890", Password = "Password123" },
                new StudentDto { Id = 2, Name = "Jane", Email = "jane@prueba.com", Phone = "9876543210", Password = "Password456" }
            };

            
            _mockStudentLogic.Setup(logic => logic.GetAllAsync())
                .ReturnsAsync(studentDtos);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<StudentDto>>(okResult.Value);

            // Verificar que la cantidad de estudiantes es la esperada
            Assert.Equal(2, returnValue.Count());

            // Verificar los datos del primer estudiante
            var firstStudent = returnValue.First();
            Assert.Equal("John", firstStudent.Name);
            Assert.Equal("john@prueba.com", firstStudent.Email);
            Assert.Equal("1234567890", firstStudent.Phone);
        }

        [Fact]
        public async Task Get_ReturnsOk_WhenStudentExists()
        {
            // Arrange
            int studentId = 1;
            var studentDto = new StudentDto
            {
                Id = studentId,
                Name = "John",
                Email = "john@prueba.com",
                Phone = "1234567890",
                Password = "Password123"
            };

            _mockStudentLogic.Setup(logic => logic.FindAsync(studentId))
                .ReturnsAsync(studentDto);

            // Act
            var result = await _controller.Get(studentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<StudentDto>(okResult.Value);

            // Verificar los datos del estudiante
            Assert.Equal(studentDto.Id, returnValue.Id);
            Assert.Equal(studentDto.Name, returnValue.Name);
            Assert.Equal(studentDto.Email, returnValue.Email);
            Assert.Equal(studentDto.Phone, returnValue.Phone);
            Assert.Equal(studentDto.Password, returnValue.Password);

            // Verificar que el método de lógica fue llamado
            _mockStudentLogic.Verify(logic => logic.FindAsync(studentId), Times.Once);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            int studentId = 1;

            // Simulamos que la lógica de negocio no encuentra al estudiante
            _mockStudentLogic.Setup(logic => logic.FindAsync(studentId))
                .ReturnsAsync((StudentDto?)null);

            // Act
            var result = await _controller.Get(studentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("El estudiante no esta registrado en la base de datos!", notFoundResult?.Value?.ToString());
        }

        [Fact]
        public async Task GetAllByCourseId_ReturnsOk_WhenStudentsExistForCourse()
        {
            // Arrange
            int courseId = 1;
            var studentDtos = new List<StudentDto>
            {
                new StudentDto
                {
                    Id = 1,
                    Name = "John",
                    Email = "john@prueba.com",
                    Phone = "1234567890",
                    Password = "Password123"
                },
                new StudentDto
                {
                    Id = 2,
                    Name = "Jane",
                    Email = "jane@prueba.com",
                    Phone = "0987654321",
                    Password = "Password456"
                }
            };

            _mockStudentLogic.Setup(logic => logic.GetAllByCourseId(courseId))
                .ReturnsAsync(studentDtos);

            // Act
            var result = await _controller.GetAllByCourseId(courseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<StudentDto>>(okResult.Value);

            // Verificar que la lista de estudiantes tiene el número correcto de elementos
            Assert.Equal(2, returnValue.Count);

            // Verificar los datos de los estudiantes
            Assert.Equal(studentDtos[0].Id, returnValue[0].Id);
            Assert.Equal(studentDtos[1].Name, returnValue[1].Name);
            Assert.Equal(studentDtos[0].Email, returnValue[0].Email);

            // Verificar que el método de lógica fue llamado
            _mockStudentLogic.Verify(logic => logic.GetAllByCourseId(courseId), Times.Once);
        }

        [Fact]
        public async Task GetAllByCourseId_ReturnsNotFound_WhenNoStudentsExistForCourse()
        {
            // Arrange
            int courseId = 1;

            // Simulamos que no se encuentran estudiantes para el curso
            _mockStudentLogic.Setup(logic => logic.GetAllByCourseId(courseId))
                .ReturnsAsync((IEnumerable<StudentDto>?)null);

            // Act
            var result = await _controller.GetAllByCourseId(courseId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("El curso no esta registrado en la base de datos!", notFoundResult?.Value?.ToString());
        }

        [Fact]
        public async Task GetCreditsByStudent_ReturnsOk_WhenStudentExists()
        {
            // Arrange
            int studentId = 1;
            var creditsStudentDto = new CreditsStudentDto
            {
                Id = 1,
                Total = 3,
                StudentId = studentId
            };

            _mockStudentLogic.Setup(logic => logic.GetCreditsByStudent(studentId))
                .ReturnsAsync(creditsStudentDto);

            // Act
            var result = await _controller.GetCreditsByStudent(studentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CreditsStudentDto>(okResult.Value);

            // Verificar los datos de los créditos
            Assert.Equal(creditsStudentDto.Id, returnValue.Id);
            Assert.Equal(creditsStudentDto.Total, returnValue.Total);
            Assert.Equal(creditsStudentDto.StudentId, returnValue.StudentId);

            // Verificar que el método de lógica fue llamado
            _mockStudentLogic.Verify(logic => logic.GetCreditsByStudent(studentId), Times.Once);
        }

        [Fact]
        public async Task GetCreditsByStudent_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            int studentId = 1;

            // Simulamos que no se encuentra al estudiante en la base de datos (retorna null)
            _mockStudentLogic.Setup(logic => logic.GetCreditsByStudent(studentId))
                .ReturnsAsync((CreditsStudentDto?)null);

            // Act
            var result = await _controller.GetCreditsByStudent(studentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("El estudiante no esta registrado en la base de datos!", notFoundResult?.Value?.ToString());
        }

       







    }
}
