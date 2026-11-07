using Uniaraxa.Domain.Entities;
using Uniaraxa.Domain.Interfaces;

namespace Uniaraxa.Infra.Repositories;

// Armazenamento em memória: os dados são perdidos ao reiniciar a aplicação.
public class TeacherRepository : ITeacherRepository
{
    private readonly Dictionary<int, Teacher> _teachers = new();
    private readonly object _sync = new();
    private int _nextId;

    public Teacher AddTeacher(Teacher teacher)
    {
        lock (_sync)
        {
            var saved = Copy(teacher);
            saved.Id = ++_nextId;
            _teachers.Add(saved.Id, saved);
            return Copy(saved);
        }
    }

    public bool UpdateTeacher(Teacher teacher)
    {
        lock (_sync)
        {
            if (!_teachers.ContainsKey(teacher.Id))
                return false;

            _teachers[teacher.Id] = Copy(teacher);
            return true;
        }
    }

    public IReadOnlyList<Teacher> GetAllTeachers()
    {
        lock (_sync)
        {
            return _teachers.Values.OrderBy(teacher => teacher.Id).Select(Copy).ToList();
        }
    }

    public Teacher? GetTeacherById(int id)
    {
        lock (_sync)
        {
            return _teachers.TryGetValue(id, out var teacher) ? Copy(teacher) : null;
        }
    }

    public bool DeleteTeacher(int id)
    {
        lock (_sync)
        {
            return _teachers.Remove(id);
        }
    }

    private static Teacher Copy(Teacher teacher) => new()
    {
        Id = teacher.Id,
        Name = teacher.Name,
        Email = teacher.Email,
        UrlLattes = teacher.UrlLattes,
        DataAdmissao = teacher.DataAdmissao
    };
}
