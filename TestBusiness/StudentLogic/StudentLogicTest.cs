using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.LoginLogic;
using Business.StudentLogic;
using DataTransferObjects;
using Entities;
using Moq;
using Repository.Repository.LoginRepository;
using Repository.Repository.StudentRepository;

namespace TestBusiness.StudentLogic
{
    public class StudentLogicTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IStudentRepository> _mockStudentRepository;
        private readonly IStudentLogic _studentLogic;

        public StudentLogicTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockStudentRepository = new Mock<IStudentRepository>();
            _studentLogic = new Business.StudentLogic.StudentLogic(_mockMapper.Object, _mockStudentRepository.Object);
        }

        [Fact]
        public async Task FindAsync_ShouldReturnMappedStudentDto_WhenStudentExists()
        {
            // Arrange
            var studentId = 1;

            var student = new Student
            {
                Id = 1,
                Name = "John",
                Email = "john@prueba.com",
                Phone = "1234567890",
                Password = "SecurePassword"
            };

            var studentDto = new StudentDto
            {
                Id = 1,
                Name = "John",
                Email = "john@prueba.com",
                Phone = "1234567890"
            };

            _mockStudentRepository
                .Setup(repo => repo.FindAsync(studentId))
                .ReturnsAsync(student);

            _mockMapper
                .Setup(mapper => mapper.Map<StudentDto>(student))
                .Returns(studentDto);

            // Act
            var result = await _studentLogic.FindAsync(studentId);

            // Assert
            _mockStudentRepository.Verify(repo => repo.FindAsync(studentId), Times.Once);

            // Verificar que el resultado es el StudentDto esperado
            Assert.Equal(studentDto.Id, result.Id);
            Assert.Equal(studentDto.Name, result.Name);
            Assert.Equal(studentDto.Email, result.Email);
            Assert.Equal(studentDto.Phone, result.Phone);
        }


        [Fact]
        public async Task FindAsync_ShouldReturnNull_WhenStudentDoesNotExist()
        {
            // Arrange
            var studentId = 999;

            _mockStudentRepository
                .Setup(repo => repo.FindAsync(studentId))
                .ReturnsAsync((Student?)null);

            // Act
            var result = await _studentLogic.FindAsync(studentId);

            // Assert
            _mockStudentRepository.Verify(repo => repo.FindAsync(studentId), Times.Once);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedStudentDtos_WhenStudentsExist()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "John", Email = "john@prueba.com", Phone = "1234567890", Password = "SecurePassword" },
                new Student { Id = 2, Name = "Jane", Email = "john@prueba.com", Phone = "0987654321", Password = "AnotherPassword" }
            };

            var studentDtos = new List<StudentDto>
            {
                new StudentDto { Id = 1, Name = "John", Email = "john@prueba.com", Phone = "1234567890" },
                new StudentDto { Id = 2, Name = "Jane", Email = "john@prueba.com", Phone = "0987654321" }
            };

            _mockStudentRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(students);

            _mockMapper
                .Setup(mapper => mapper.Map<List<StudentDto>>(students))
                .Returns(studentDtos);

            // Act
            var result = await _studentLogic.GetAllAsync();

            // Assert
            _mockStudentRepository.Verify(repo => repo.GetAllAsync(), Times.Once);

            Assert.Equal(students.Count, result.Count());

            Assert.Equal(studentDtos[0].Id, result.ElementAt(0).Id);
            Assert.Equal(studentDtos[0].Name, result.ElementAt(0).Name);
            Assert.Equal(studentDtos[0].Email, result.ElementAt(0).Email);
            Assert.Equal(studentDtos[0].Phone, result.ElementAt(0).Phone);

            Assert.Equal(studentDtos[1].Id, result.ElementAt(1).Id);
            Assert.Equal(studentDtos[1].Name, result.ElementAt(1).Name);
            Assert.Equal(studentDtos[1].Email, result.ElementAt(1).Email);
            Assert.Equal(studentDtos[1].Phone, result.ElementAt(1).Phone);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoStudentsExist()
        {
            // Arrange
            var students = new List<Student>();
            var studentDtos = new List<StudentDto>();

            _mockStudentRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(students);

            _mockMapper
                .Setup(mapper => mapper.Map<List<StudentDto>>(students))
                .Returns(studentDtos);

            // Act
            var result = await _studentLogic.GetAllAsync();

            // Assert
            _mockStudentRepository.Verify(repo => repo.GetAllAsync(), Times.Once);

            // Verificar que el resultado es una lista vacía
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllByCourseId_ShouldReturnMappedStudentDtos_WhenStudentsExistInCourse()
        {
            // Arrange
            int courseId = 1;  // El courseId que estamos buscando
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "John", Email = "john@prueba.com", Phone = "1234567890", Password = "SecurePassword"},
                new Student { Id = 2, Name = "Jane", Email = "john@prueba.com", Phone = "0987654321", Password = "AnotherPassword"}
            };

            var studentDtos = new List<StudentDto>
            {
                new StudentDto { Id = 1, Name = "John", Email = "john@prueba.com", Phone = "1234567890" },
                new StudentDto { Id = 2, Name = "Jane", Email = "john@prueba.com", Phone = "0987654321" }
            };

            _mockStudentRepository
                .Setup(repo => repo.GetAllByCourseId(courseId))
                .ReturnsAsync(students);

            _mockMapper
                .Setup(mapper => mapper.Map<List<StudentDto>>(students))
                .Returns(studentDtos);

            // Act
            var result = await _studentLogic.GetAllByCourseId(courseId);

            // Assert
            _mockStudentRepository.Verify(repo => repo.GetAllByCourseId(courseId), Times.Once);

            // Verificar que el resultado contiene la misma cantidad de estudiantes mapeados
            Assert.Equal(students.Count, result.Count());

            // Verificar que los datos de los StudentDto sean correctos
            Assert.Equal(studentDtos[0].Id, result.ElementAt(0).Id);
            Assert.Equal(studentDtos[0].Name, result.ElementAt(0).Name);
            Assert.Equal(studentDtos[0].Email, result.ElementAt(0).Email);
            Assert.Equal(studentDtos[0].Phone, result.ElementAt(0).Phone);

            Assert.Equal(studentDtos[1].Id, result.ElementAt(1).Id);
            Assert.Equal(studentDtos[1].Name, result.ElementAt(1).Name);
            Assert.Equal(studentDtos[1].Email, result.ElementAt(1).Email);
            Assert.Equal(studentDtos[1].Phone, result.ElementAt(1).Phone);
        }

        [Fact]
        public async Task GetAllByCourseId_ShouldReturnEmptyList_WhenNoStudentsExistInCourse()
        {
            // Arrange
            int courseId = 1; 
            var students = new List<Student>();
            var studentDtos = new List<StudentDto>();

            _mockStudentRepository
                .Setup(repo => repo.GetAllByCourseId(courseId))
                .ReturnsAsync(students);

            _mockMapper
                .Setup(mapper => mapper.Map<List<StudentDto>>(students))
                .Returns(studentDtos);

            // Act
            var result = await _studentLogic.GetAllByCourseId(courseId);

            // Assert
            _mockStudentRepository.Verify(repo => repo.GetAllByCourseId(courseId), Times.Once);
            
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCreditsByStudent_ShouldReturnMappedCreditsStudentDto_WhenCreditsExist()
        {
            // Arrange
            int studentId = 1;
            var creditsStudent = new CreditsStudent
            {
                Id = 1,
                Total = 9,
                StudentId = studentId,
                Student = new Student { Id = studentId, Name = "John", Email = "john@prueba.com", Phone = "1234567890", Password = "SecurePassword" }
            };

            var creditsStudentDto = new CreditsStudentDto
            {
                Id = 1,
                Total = 30,
                StudentId = studentId
            };

            _mockStudentRepository
                .Setup(repo => repo.GetCreditsByStudent(studentId))
                .ReturnsAsync(creditsStudent);

            _mockMapper
                .Setup(mapper => mapper.Map<CreditsStudentDto>(creditsStudent))
                .Returns(creditsStudentDto);

            // Act
            var result = await _studentLogic.GetCreditsByStudent(studentId);

            // Assert
            _mockStudentRepository.Verify(repo => repo.GetCreditsByStudent(studentId), Times.Once);

            // Verificar que el resultado contiene los datos correctos
            Assert.Equal(creditsStudentDto.Id, result.Id);
            Assert.Equal(creditsStudentDto.Total, result.Total);
            Assert.Equal(creditsStudentDto.StudentId, result.StudentId);
        }

        [Fact]
        public async Task RemoveAsync_ShouldReturnMappedStudentDto_WhenStudentExists()
        {
            // Arrange
            int studentId = 1; 
            var student = new Student
            {
                Id = studentId,
                Name = "John",
                Email = "john@prueba.com",
                Phone = "1234567890",
                Password = "SecurePassword"
            };

            var studentDto = new StudentDto
            {
                Id = studentId,
                Name = "John",
                Email = "john@prueba.com",
                Phone = "1234567890"
            };

            _mockStudentRepository
                .Setup(repo => repo.RemoveAsync(studentId))
                .ReturnsAsync(student);

            _mockMapper
                .Setup(mapper => mapper.Map<StudentDto>(student))
                .Returns(studentDto);

            // Act
            var result = await _studentLogic.RemoveAsync(studentId);

            // Assert
            _mockStudentRepository.Verify(repo => repo.RemoveAsync(studentId), Times.Once);

            // Verificar que el resultado contiene los datos correctos
            Assert.Equal(studentDto.Id, result.Id);
            Assert.Equal(studentDto.Name, result.Name);
            Assert.Equal(studentDto.Email, result.Email);
            Assert.Equal(studentDto.Phone, result.Phone);
        }

        [Fact]
        public async Task RemoveAsync_ShouldReturnNull_WhenStudentDoesNotExist()
        {
            // Arrange
            int studentId = 1; 

            _mockStudentRepository.Setup(repo => repo.RemoveAsync(studentId)).ReturnsAsync((Student?)null);

            // Act
            var result = await _studentLogic.RemoveAsync(studentId);

            // Assert
            _mockStudentRepository.Verify(repo => repo.RemoveAsync(studentId), Times.Once);

            // Verificar que el resultado sea null cuando el estudiante no exista
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnMappedStudentDto_WhenStudentExists()
        {
            // Arrange
            int studentId = 1; 
            var studentDto = new StudentDto
            {
                Id = studentId,
                Name = "John Updated",
                Email = "john.updated@prueba.com",
                Phone = "0987654321"
            };

            var student = new Student
            {
                Id = studentId,
                Name = "John Updated",
                Email = "john.updated@prueba.com",
                Phone = "0987654321",
                Password = "UpdatedPassword"
            };

            var updatedStudentDto = new StudentDto
            {
                Id = studentId,
                Name = "John Updated",
                Email = "john.updated@prueba.com",
                Phone = "0987654321"
            };

            _mockStudentRepository
                .Setup(repo => repo.UpdateAsync(studentId, It.IsAny<Student>()))
                .ReturnsAsync(student);

            _mockMapper
                .Setup(mapper => mapper.Map<StudentDto>(student))
                .Returns(updatedStudentDto);

            // Act
            var result = await _studentLogic.UpdateAsync(studentId, studentDto);

            // Assert
            _mockStudentRepository.Verify(repo => repo.UpdateAsync(studentId, It.IsAny<Student>()), Times.Once);

            // Verificar que el resultado contiene los datos correctos
            Assert.Equal(updatedStudentDto.Id, result.Id);
            Assert.Equal(updatedStudentDto.Name, result.Name);
            Assert.Equal(updatedStudentDto.Email, result.Email);
            Assert.Equal(updatedStudentDto.Phone, result.Phone);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenStudentDoesNotExist()
        {
            // Arrange
            int studentId = 1;
            var studentDto = new StudentDto
            {
                Id = studentId,
                Name = "John Updated",
                Email = "john.updated@prueba.com",
                Phone = "0987654321"
            };

            _mockStudentRepository
                .Setup(repo => repo.UpdateAsync(studentId, It.IsAny<Student>()))
                .ReturnsAsync((Student?)null);

            // Act
            var result = await _studentLogic.UpdateAsync(studentId, studentDto);

            // Assert
            _mockStudentRepository.Verify(repo => repo.UpdateAsync(studentId, It.IsAny<Student>()), Times.Once);

            // Verificar que el resultado sea null cuando el estudiante no exista
            Assert.Null(result);
        }
    }
}
