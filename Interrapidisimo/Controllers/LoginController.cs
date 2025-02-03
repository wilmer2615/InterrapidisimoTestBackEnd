using Business.LoginLogic;
using DataTransferObjects;
using Interrapidisimo.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interrapidisimo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly ILoginLogic _loginLogic;

        private readonly IAuthentication _authentication;
        public LoginController(ILoginLogic loginLogic, IAuthentication authentication)
        {
            this._loginLogic = loginLogic;
            this._authentication = authentication;
        }

        /// <summary>
        /// Accion que permite hacer la creacion de un nuevo estudiante.
        /// </summary>
        /// <param name="studentDto">Informacion del nuevo estudiante.</param>
        /// <returns>Nuevo estudiante agregado.</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] StudentDto studentDto)
        {
            // Se realiza la validacion del modelo.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _loginLogic.AddAsync(studentDto));
        }

        /// <summary>
        /// Acción que permite verificar la cuenta del estudiante.
        /// </summary>
        /// <param name="accountDto">Información de la cuenta del estudiante (correo y contraseña).</param>
        /// <returns>Verdadero o falso dependiendo de si la verificación fue exitosa.</returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] AccountDto accountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _loginLogic.VerifyAccountAsync(accountDto);

            if (result == null)
                return NotFound(new { Message = "Datos incorrectos!" });
            else
                return Ok(new { Token = this._authentication.GenerateToken(result) });

        }
    }
}
