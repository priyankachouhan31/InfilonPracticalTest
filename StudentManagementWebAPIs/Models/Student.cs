using System;
using System.Collections.Generic;

namespace StudentManagementWebAPIs.Models;

public partial class Students
{
    public int StudentId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? EmailId { get; set; }

    public string? ClassIds { get; set; }
}
