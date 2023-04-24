using WebApiMaxine.Common;
using WebApiMaxine.Interface;
using WebApiMaxine.Requset;
using WebApiMaxine.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleCaptcha; //圖形驗證碼
using WebApiMaxine.ErrorHandling;
using WebApiMaxine.Enums;

namespace WebApiMaxine.Controllers;

[AllowAnonymous]//但這麼一來還有一個問題，就是變成我們登入的api也不能使用了，所以這邊就需要在我們登入的api上方加上[AllowAnonymous]。
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;
    private readonly ICaptcha _captcha;

    public AccountController(ILogger<AccountController> logger, IAccountService accountService, ICaptcha captcha)
    {
        _logger = logger;
        _accountService = accountService;
        _captcha = captcha;
    }

    /// <summary>
    /// 登入
    /// </summary>
    /// <returns></returns>
    [HttpPost("Login")]
    public async Task<ResponseResult<LoginResp>> Login(AuthenticateModel requset)
    {
        if (!Validate(requset.Captcha))
            throw new BusinessException(BusinessError.LoginFailed, "驗證碼錯誤");

        var content = await _accountService.LoginAsync(requset);

        return new ResponseResult<LoginResp>() { RtnCode = (int)ReturnCodes.CODE_SUCCESS, Msg = MsgCodes.Msg_00, Data = content };
    }

    [HttpGet("Captcha")] //驗證碼
    public IActionResult Captcha()
    {
        var info = _captcha.Generate("test");
        var stream = new MemoryStream(info.CaptchaByteData);
        return File(stream, "image/png");
    }

    private bool Validate(string code)
    {
        return _captcha.Validate("test", code);            
    }

    /// <summary>
    /// 新增會員
    /// </summary>
    /// <returns></returns>
    [HttpPost("CreateUser")]
    public async Task<ResponseResult<object>> CreateUser(CreateUserReq requset)
    {
        await _accountService.InsertUserAsync(requset);

        return new ResponseResult<object>() { RtnCode = (int)ReturnCodes.CODE_SUCCESS, Msg = MsgCodes.Msg_00 };
    }
}
