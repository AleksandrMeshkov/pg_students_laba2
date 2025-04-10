using System;
using System.Collections.Generic;

namespace laba2_db.models;

public partial class Grade
{
    public int Gradeid { get; set; }

    public int? Studentid { get; set; }

    public int? Physicsgrade { get; set; }

    public int? Mathgrade { get; set; }

    public virtual Student? Student { get; set; }
}
