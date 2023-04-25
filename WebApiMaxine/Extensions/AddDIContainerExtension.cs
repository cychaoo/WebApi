using WebApiMaxine.Interface;
using WebApiMaxine.Services;

namespace WebApiMaxine.Extensions;

public static class AddDIContainerExtension
{
    public static IServiceCollection AddDiContainer(this IServiceCollection services)
    {
        #region 注入 Dependency Injection
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IADService, ADService>();
        services.AddScoped<IJWTService, JWTService>();
        services.AddSingleton<IRedisManageService, RedisManageService>();
        services.AddScoped<IX509CertService, X509CertService>();
        #endregion
        return services;
    }
}
