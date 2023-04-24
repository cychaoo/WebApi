using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMaxine.ErrorHandling;

/// <summary>
/// 標示錯誤類型
/// </summary>
[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
public class ErrorCatalogAttribute : Attribute
{
    public ErrorCodeType ErrorType { get; set; }

    public ErrorCatalogAttribute(ErrorCodeType errorType)
    {
        ErrorType = errorType;
    }
}
