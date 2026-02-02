using System;
using System.Collections.Generic;

namespace StudentManagementWebAPIs.Models;

public partial class Classes
{
    public int ClassId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? StudentIds { get; set; }
}
