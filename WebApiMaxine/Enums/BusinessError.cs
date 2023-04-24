using WebApiMaxine.ErrorHandling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMaxine.Enums;

/// <summary>
/// 商業邏輯錯誤
/// </summary>
[ErrorCatalog(ErrorCodeType.Business)]
public enum BusinessError
{
    [Description("data not found")]
    NoData = 2001,

    [Description("activity can not delete")]
    ActCanNotDel = 2001,

    [Description("export exccel error")]
    ExportError = 2002,

    [Description("login failed")]
    LoginFailed = 2003,

    [Description("userId is exist")]
    UserIdIsExist = 2004,

    [Description("data is error")]
    DataError = 2005,
}
