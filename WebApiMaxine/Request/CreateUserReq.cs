using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMaxine.Requset;

public class CreateUserReq
{
    [Required]
    //[RegularExpression(@"^[sS][0-9]{5}|[mM][0-9]{5}$", ErrorMessage = "帳號不符合格式")]
    [StringLength(6)]
    public string UserId { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string UserName { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string UserWord { get; set; } = null!;
}
