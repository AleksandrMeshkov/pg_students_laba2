using System;
using System.Collections.Generic;

namespace laba2_db.models;

public partial class Student
{
    public int StudentId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public DateOnly Birthdate { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
