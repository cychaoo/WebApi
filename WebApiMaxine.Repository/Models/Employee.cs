using System;
using System.Collections.Generic;

namespace WebApiMaxine.Repository.Models;

public partial class Employee
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public int? Salary { get; set; }
}
