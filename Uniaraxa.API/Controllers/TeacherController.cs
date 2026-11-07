using Microsoft.AspNetCore.Mvc;
using Uniaraxa.Domain.Entities;
using Uniaraxa.Service.Services;

namespace Uniaraxa.API.Controllers;

[ApiController]
[Route("api/teacher")]
public class TeacherController : ControllerBase
{
    private readonly ITeacherService _service;

    public TeacherController(ITeacherService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<Teacher>> GetAllTeachers()
    {
        return Ok(_service.GetAllTeachers());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Teacher> GetTeacherById(int id)
    {
        var teacher = _service.GetTeacherById(id);
        return teacher is null ? NotFound() : Ok(teacher);
    }

    [HttpPost]
    public ActionResult<Teacher> AddTeacher([FromBody] Teacher teacher)
    {
        if (teacher.DataAdmissao == default)
            return BadRequest("dataAdmissao é obrigatória.");

        var created = _service.AddTeacher(teacher);
        return CreatedAtAction(nameof(GetTeacherById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateTeacher(int id, [FromBody] Teacher teacher)
    {
        if (teacher.Id != 0 && teacher.Id != id)
            return BadRequest("O Id do corpo deve corresponder ao Id da URL.");
        if (teacher.DataAdmissao == default)
            return BadRequest("dataAdmissao é obrigatória.");

        teacher.Id = id;
        return _service.UpdateTeacher(teacher) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteTeacher(int id)
    {
        return _service.DeleteTeacher(id) ? NoContent() : NotFound();
    }
}
