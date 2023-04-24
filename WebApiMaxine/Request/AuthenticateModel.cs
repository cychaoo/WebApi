using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMaxine.Requset;

/// <summary>
///     登入 module
/// </summary>
public class AuthenticateModel
{
    /// <summary>
    ///     登入帳號
    /// </summary>
    [Required]
    //[RegularExpression(@"^[sS][0-9]{5}|[mM][0-9]{5}$", ErrorMessage = "帳號不符合格式")]
    public string UserId { get; set; } = null!;

    /// <summary>
    ///     登入密碼
    /// </summary>
    [Required]
    public string UserWord { get; set; } = null!;

    /// <summary>
    ///     驗證碼
    /// </summary>
    [Required]
    public string Captcha { get; set; } = null!;
}
