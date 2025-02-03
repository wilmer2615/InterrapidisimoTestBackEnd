using Business.StudentLogic;
using DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interrapidisimo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentLogic _studentLogic;
        public StudentController(IStudentLogic studentLogic)
        {
            this._studentLogic = studentLogic;
        }

        /// <summary>
        /// Accion que permite hacer la actualizacion de un estudiante.
        /// </summary>
        /// <param name="studentDto">Informacion actualizada del estudiante .</param>
        /// <param name="id">Identificador del estudiante a modificar.</param>
        /// <returns>Estudiante actualizado.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] StudentDto studentDto)
        {

            if (!ModelState.IsValid || id <= 0)
            {
                return BadRequest(ModelState);
            }

            var result = await _studentLogic.UpdateAsync(id, studentDto);

            if (result != null)
            {
                return Ok();
            }
            return NotFound(new { Message = "El estudiante no esta registrado en la base de datos!" });
        }

        /// <summary>
        /// Accion que permite listar los estudiantes.
        /// </summary>
        /// <returns>Lista de estudiantes.</returns>
        [HttpGet()]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _studentLogic.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Accion que permite obtener un estudiante por Id.
        /// <param name="id">Identificador del estudiante.</param>
        /// </summary>
        /// <returns>Estudiante seleccionado.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _studentLogic.FindAsync(id);
            if (result == null)
                return NotFound(new { Message = "El estudiante no esta registrado en la base de datos!" });

            return Ok(result);
        }

        /// <summary>
        /// Accion que permite obtener un estudiante por Id.
        /// <param name="courseId">Identificador del estudiante.</param>
        /// </summary>
        /// <returns>Estudiante seleccionado.</returns>
        [HttpGet("students-by-course/{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByCourseId(int courseId)
        {
            var result = await _studentLogic.GetAllByCourseId(courseId);
            if (result == null)
                return NotFound(new { Message = "El curso no esta registrado en la base de datos!" });

            return Ok(result);
        }

        /// <summary>
        /// Accion que permite obtener un estudiante por Id.
        /// <param name="studentId">Identificador del estudiante.</param>
        /// </summary>
        /// <returns>Estudiante seleccionado.</returns>
        [HttpGet("credits-student/{studentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCreditsByStudent(int studentId)
        {
            var result = await _studentLogic.GetCreditsByStudent(studentId);
            if (result == null)
                return NotFound(new { Message = "El estudiante no esta registrado en la base de datos!" });

            return Ok(result);
        }
    }
}
