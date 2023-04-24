using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMaxine.Response;

public class LoginResp
{
    /// <summary>
    /// 使用者帳號
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// 使用者名稱
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Access Toekn
    /// </summary>
    public string ?Token { get; set; }
}
