using Business.LoginLogic;
using DataTransferObjects;
using Interrapidisimo.Controllers;
using Interrapidisimo.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json.Linq;

namespace TestInterrapidisimo.Controllers
{
    public class LoginControllerTest
    {
        private readonly Mock<ILoginLogic> _mockLoginLogic;
        private readonly Mock<IAuthentication> _mockAuthentication;
        private readonly LoginController _controller;

        public LoginControllerTest()
        {
            // Mock de ILoginLogic
            _mockLoginLogic = new Mock<ILoginLogic>();

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("TestKey");

            _mockAuthentication = new Mock<IAuthentication>();
            _mockAuthentication.Setup(auth => auth.GenerateToken(It.IsAny<StudentDto>())).Returns("TestToken");

            _controller = new LoginController(_mockLoginLogic.Object, _mockAuthentication.Object);
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenStudentDtoIsValid()
        {
            // Arrange
            var studentDto = new StudentDto
            {
                Name = "John",
                Email = "john@prueba.com",
                Phone = "1234567890",
                Password = "Password123"
            };

            var expectedStudentDto = new StudentDto
            {
                Name = "John",
                Email = "john@prueba.com",
                Phone = "1234567890",
                Password = "Password123"
            };


            _mockLoginLogic.Setup(logic => logic.AddAsync(studentDto))
                .ReturnsAsync(expectedStudentDto);

            // Act
            var result = await _controller.Register(studentDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<StudentDto>(okResult.Value);

            // Verifica que los valores
            Assert.Equal(expectedStudentDto.Name, returnValue.Name);
            Assert.Equal(expectedStudentDto.Email, returnValue.Email);
            Assert.Equal(expectedStudentDto.Phone, returnValue.Phone);
            Assert.Equal(expectedStudentDto.Password, returnValue.Password);

            // Verificar que el método de lógica fue llamado
            _mockLoginLogic.Verify(logic => logic.AddAsync(studentDto), Times.Once);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var studentDto = new StudentDto();

            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Register(studentDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            // Verificar que el método de lógica no fue llamado
            _mockLoginLogic.Verify(logic => logic.AddAsync(It.IsAny<StudentDto>()), Times.Never);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            // Arrange
            var accountDto = new AccountDto
            {
                Email = "prueba@prueba.com",
                Password = "Password123"
            };

            var studentDto = new StudentDto
            {
                Id = 1,
                Name = "Jonh",
                Email = "prueba@prueba.com",
                Phone = "1234567890",
                Password = "Password123"
            };
                        
            _mockLoginLogic.Setup(logic => logic.VerifyAccountAsync(accountDto)).ReturnsAsync(studentDto);

            var token = "mock-token";

            _mockAuthentication.Setup(auth => auth.GenerateToken(studentDto)).Returns(token);

            // Act
            var result = await _controller.Login(accountDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Usamos Newtonsoft.Json para acceder a la propiedad Token
            var jsonResponse = JObject.FromObject(okResult.Value);
            var returnToken = jsonResponse["Token"]?.ToString();

            Assert.Equal(token, returnToken);

        }




    }
}
