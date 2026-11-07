using Uniaraxa.Domain.Entities;
using Uniaraxa.Domain.Interfaces;

namespace Uniaraxa.Service.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _repository;

    public TeacherService(ITeacherRepository repository)
    {
        _repository = repository;
    }

    public Teacher AddTeacher(Teacher teacher) => _repository.AddTeacher(teacher);

    public bool UpdateTeacher(Teacher teacher) => _repository.UpdateTeacher(teacher);

    public IReadOnlyList<Teacher> GetAllTeachers() => _repository.GetAllTeachers();

    public Teacher? GetTeacherById(int id) => _repository.GetTeacherById(id);

    public bool DeleteTeacher(int id) => _repository.DeleteTeacher(id);
}
