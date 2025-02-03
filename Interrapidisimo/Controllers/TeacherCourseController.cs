using Business.TeacherCourseLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interrapidisimo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherCourseController : ControllerBase
    {
        private readonly ITeacherCourseLogic _teacherCourseLogic;

        public TeacherCourseController(ITeacherCourseLogic teacherCourseLogic)
        {
            this._teacherCourseLogic = teacherCourseLogic;
        }

        /// <summary>
        /// Accion que permite listar los registros.
        /// </summary>
        /// <returns>Lista de registros.</returns>
        [HttpGet("{studentId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCourseByTeacher(int studentId)
        {
            var result = await _teacherCourseLogic.GetCourseByTeacher(studentId);
            return Ok(result);
        }
    }
}
