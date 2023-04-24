using System;
using System.Collections.Generic;

namespace WebApiMaxine.Repository.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public string? UserWord { get; set; }
}
