using AutoMapper;
using Business.LoginLogic;
using DataTransferObjects;
using Entities;
using Moq;
using Repository.Repository.LoginRepository;

namespace TestBusiness.LoginLogic
{
    public class LoginLogicTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoginRepository> _mockLoginRepository;
        private readonly ILoginLogic _loginLogic;

        public LoginLogicTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockLoginRepository = new Mock<ILoginRepository>();
            _loginLogic = new Business.LoginLogic.LoginLogic(_mockMapper.Object, _mockLoginRepository.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnMappedStudentDto_WhenStudentIsAdded()
        {
            // Arrange
            var studentDto = new StudentDto
            {
                Name = "Jonh",
                Email = "jonh@prueba.com",
                Phone = "1234567890",
                Password = "Password123"
            };

            var student = new Student
            {
                Id = 1,
                Name = "Jonh",
                Email = "jonh@prueba.com",
                Phone = "1234567890",
                Password = "Password123"
            };

            _mockLoginRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Student>()))
                .ReturnsAsync(student);

            _mockMapper
                .Setup(mapper => mapper.Map<StudentDto>(It.IsAny<Student>()))
                .Returns(studentDto);

            // Act
            var result = await _loginLogic.AddAsync(studentDto);

            // Assert
            _mockLoginRepository.Verify(repo => repo.AddAsync(It.IsAny<Student>()), Times.Once);

            // Verificar que el resultado sea el esperado
            Assert.Equal(studentDto.Name, result.Name);
            Assert.Equal(studentDto.Email, result.Email);
            Assert.Equal(studentDto.Phone, result.Phone);
            Assert.Equal(studentDto.Password, result.Password);
        }

        [Fact]
        public async Task VerifyAccountAsync_ShouldReturnStudentDto_WhenAccountIsValid()
        {
            // Arrange
            var accountDto = new AccountDto
            {
                Email = "jonh@prueba.com",
                Password = "Password123"
            };

            var student = new Student
            {
                Id = 1,
                Name = "Jonh",
                Email = "jonh@prueba.com",
                Phone = "1234567890",
                Password = "Password123"
            };

            var studentDto = new StudentDto
            {
                Id = 1,
                Name = "Jonh",
                Email = "jonh@prueba.com",
                Phone = "1234567890"
            };

            _mockLoginRepository
                .Setup(repo => repo.VerifyAccountAsync(accountDto.Email, accountDto.Password))
                .ReturnsAsync(student);

            _mockMapper
                .Setup(mapper => mapper.Map<StudentDto>(student))
                .Returns(studentDto);

            // Act
            var result = await _loginLogic.VerifyAccountAsync(accountDto);

            // Assert            
            _mockLoginRepository.Verify(repo => repo.VerifyAccountAsync(accountDto.Email, accountDto.Password), Times.Once);


            // Verificar que el resultado es el esperado
            Assert.Equal(studentDto.Id, result?.Id);
            Assert.Equal(studentDto.Name, result?.Name);
            Assert.Equal(studentDto.Email, result?.Email);
            Assert.Equal(studentDto.Phone, result?.Phone);
        }
    }
}
