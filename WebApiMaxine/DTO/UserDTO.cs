using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net6Api.DTO;

public class UserDTO
{
    /// <summary>
    /// 編號
    /// </summary>
    public int RowId { get; set; }

    /// <summary>
    /// 使用者帳號
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// 使用者名稱
    /// </summary>
    public string? UserName { get; set; }
}
