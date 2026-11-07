using Uniaraxa.Domain.Entities;

namespace Uniaraxa.Service.Services;

public interface ITeacherService
{
    Teacher AddTeacher(Teacher teacher);
    bool UpdateTeacher(Teacher teacher);
    IReadOnlyList<Teacher> GetAllTeachers();
    Teacher? GetTeacherById(int id);
    bool DeleteTeacher(int id);
}
