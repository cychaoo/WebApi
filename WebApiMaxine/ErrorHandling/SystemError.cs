namespace WebApiMaxine.ErrorHandling;

/// <summary>
/// 系統相關錯誤
/// </summary>
[ErrorCatalog(ErrorCodeType.System)]
public enum SystemError
{
    UnknowError,

    EnumSettingError,
}
