using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using WebApiMaxine.Common;
using WebApiMaxine.Repository.Models;

namespace WebApiMaxine.Filter;

public class AuthorizationFilter : IAsyncAuthorizationFilter
{
    private readonly ILogger<AuthorizationFilter> _logger;
    public AuthorizationFilter(ILogger<AuthorizationFilter> logger)
    {
        _logger = logger;
    }
    /// <summary>
    /// 當使用者被驗證成功進入該事件
    /// </summary>
    /// <param name="context"></param>
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            //取得Windows使用者
            string user = context.HttpContext.User.Identity!.Name!;

            //將Windows使用者及驗證結果放入Claim物件中，可在WEB API內使用
            context.HttpContext!.User = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                            new Claim("User", user),
                            //new Claim("LoginState",ReturnCodes.CODE_SUCCESS.ToString())
                    }));

        }
        catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }
}
