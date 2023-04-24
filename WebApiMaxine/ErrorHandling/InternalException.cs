using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMaxine.ErrorHandling;

/// <summary>
/// 共用元件發生例外狀況
/// </summary>
public class InternalException : BaseException
{
    public InternalException(Enum errorEnum) : base(errorEnum)
    {
    }

    public InternalException(Enum errorEnum, string message) : base(errorEnum, message)
    {
    }
}
