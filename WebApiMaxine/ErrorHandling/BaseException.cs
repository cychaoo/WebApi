using System.ComponentModel.DataAnnotations;

namespace WebApiMaxine.ErrorHandling;

/// <summary>
/// 系統例外狀況基底
/// </summary>
public abstract class BaseException : Exception
{
    public Enum ErrorEnum { get; set; }

    public BaseException(Enum errorEnum)
    {
        ErrorEnum = errorEnum;
    }

    public BaseException(Enum errorEnum, string message) : base(message)
    {
        ErrorEnum = errorEnum;
    }

    public string GetErrorCode()
    {
        var errorType = ErrorEnum.GetType()
            .GetCustomAttributes(true)
            .OfType<ErrorCatalogAttribute>()
            .SingleOrDefault();

        if (errorType == null)
        {
            throw new InternalException(SystemError.EnumSettingError, $"{ErrorEnum.GetType().Name} need to use ErrorTypeAttribute.");
        }

        var displayAttribute = errorType.ErrorType.GetType()
            .GetMember(errorType.ErrorType.ToString())
            .First()
            .GetCustomAttributes(true)
            .OfType<DisplayAttribute>()
            .First();

        return $"{displayAttribute.Name}.{ErrorEnum}";
    }
}
