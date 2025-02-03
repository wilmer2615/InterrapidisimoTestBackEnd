using Business.RegisteredCourseLogic;
using DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interrapidisimo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisteredCourseController : ControllerBase
    {
        private readonly IRegisteredCorseLogic _registeredCourseLogic;

        public RegisteredCourseController(IRegisteredCorseLogic registeredCorseLogic)
        {
            this._registeredCourseLogic = registeredCorseLogic;
        }

        /// <summary>
        /// Accion que permite registrar materias a un estudiante.
        /// </summary>
        /// <param name="registeredCourseDto">Informacion del nuevo registro.</param>
        /// <returns>Nuevo registro agregado.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisteredCourseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] RegisteredCourseDto registeredCourseDto)
        {
            // Se realiza la validacion del modelo.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _registeredCourseLogic.AddAsync(registeredCourseDto));
        }

        /// <summary>
        /// Accion que permite la eliminacion de un registro.
        /// </summary>
        /// <param name="id">Identificador del registro a eliminar.</param>
        /// <returns>Registro eliminado.</returns>
        [HttpDelete("{studentId}/{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Remove(int studentId, int courseId)
        {
            // Se realiza la validacion.
            if (studentId <= 0 | courseId <= 0)
            {
                return BadRequest(ModelState);
            }

            var result = await _registeredCourseLogic.RemoveAsync(studentId, courseId);

            if (result != null)
            {
                return Ok();
            }
            return NotFound(new { Message = "El registro no se encuentra en la base de datos!" });
        }

        /// <summary>
        /// Accion que permite listar los registros.
        /// </summary>
        /// <returns>Lista de registros.</returns>
        [HttpGet()]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _registeredCourseLogic.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Accion que permite obtener los registros por Id.
        /// <param name="id">Identificador de los registros.</param>
        /// </summary>
        /// <returns>Regsitros seleccionados.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllById(int id)
        {
            var result = await _registeredCourseLogic.FindAsync(id);
            if (result == null)
                return NotFound(new { Message = "El registro no se encuentra en la base de datos!" });

            return Ok(result);
        }
    }
}
