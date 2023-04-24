using WebApiMaxine.Enums;
using WebApiMaxine.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMaxine.ErrorHandling;

/// <summary>
/// 商業邏輯例外狀況
/// </summary>
public class BusinessException : BaseException
{
    public BusinessException(Enum errorEnum) : base(errorEnum)
    {
    }

    public BusinessException(Enum errorEnum, string message) : base(errorEnum, message)
    {
    }
}
