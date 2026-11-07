using Uniaraxa.Domain.Entities;

namespace Uniaraxa.Domain.Interfaces;

public interface ITeacherRepository
{
    Teacher AddTeacher(Teacher teacher);
    bool UpdateTeacher(Teacher teacher);
    IReadOnlyList<Teacher> GetAllTeachers();
    Teacher? GetTeacherById(int id);
    bool DeleteTeacher(int id);
}
