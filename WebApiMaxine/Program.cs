using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Text;
using WebApiMaxine.Common;
using WebApiMaxine.Extensions;
using WebApiMaxine.Filter;
using WebApiMaxine.DTO;
using WebApiMaxine.Interface;
using WebApiMaxine.Services;
using WebApiMaxine.Repository.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment.EnvironmentName; //取環境名稱
var aesKey = builder.Configuration.GetValue<string>("AES_KEY") ?? throw new ArgumentNullException("AES_KEY");
var ivKey = builder.Configuration.GetValue<string>("IV_KEY") ?? throw new ArgumentNullException("IV_KEY"); // [新增]ivKey用json取值，byMaxine

// 透過 builder.Services 將服務加入 DI 容器
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(AuthorizationFilter)); // 新增驗證授權
}); ;
builder.Logging.AddNLog("nlog.config");

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "WebApiMaxine", Version = "v1" });
});

// Add custom configuration provider.
builder.Configuration.AddEncryptConfProvider($"appsettings.{env}.json", aesKey, ivKey, true);

// Bind configurations.
var databaseConfigDetails = builder.Configuration.GetSection("Database").Get<DatabaseConfigDetail[]>();
builder.Services.Configure<DatabaseConfig>(options => options.Details = databaseConfigDetails!);
SystemShared.Init(); //初始化設定
//格式化SQL連線資訊字串
var ConnectionString = SystemShared.BuildConnectionString(databaseConfigDetails);  //[2022/12/02 調整]: 加入參數，共用連線資訊
//TODO:Added by Roger:註冊Session功能
builder.Services.AddSession();
//新增EF Core資料庫服務
builder.Services.AddDbContext<DBContext>(option => option.UseSqlServer(ConnectionString));
builder.Services.AddTransient<DBContext>();

builder.Services.Configure<RedisConfigDTO>(builder.Configuration.GetSection("Redis"));
builder.Services.AddDiContainer();

#region SimpleCache 圖形驗證碼
builder.Services.AddMemoryCache()
.AddSimpleCaptcha(builder =>
{
    builder.UseMemoryStore();
});
#endregion

#region JWT Token驗證
var audienceConfig = builder.Configuration.GetSection("JwtToken");
var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audienceConfig["Secret"]));
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,
    ValidateIssuer = true,
    ValidIssuer = audienceConfig["Issuer"],
    ValidateAudience = true,
    ValidAudience = audienceConfig["Audience"],
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero,
    RequireExpirationTime = true,
};

builder.Services.AddMvc(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})


.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
{
    x.RequireHttpsMetadata = false;//取得或設定中繼資料位址或授權單位是否需要 HTTPS。 預設值是 true。 這應該只在開發環境中停用。
    x.TokenValidationParameters = tokenValidationParameters;//設定驗證的參數
    x.Events = new JwtBearerEvents
    {
        //Token過期
        OnAuthenticationFailed = context =>
        {
            context.Response.StatusCode = 401;
            context.HttpContext.Response.WriteAsync("Lifetime validation failed. The token is expired.");

            return Task.CompletedTask;
        },
        //權限驗證失敗後觸發的事件
        OnChallenge = context =>
        {
            context.HandleResponse();

            if (context.Response.StatusCode == 200)  //200 Status: OK
            {
                context.Response.StatusCode = 401;
                context.HttpContext.Response.WriteAsync("Not Found Token. Request Access Denied");
            }

            return Task.CompletedTask;
        }
    };
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => builder.Configuration.Bind("CookieSettings", options));
#endregion


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiMaxine v1"));
}
app.UseHttpLogging(); //可完整記錄Http要求/回應內容

app.UseHttpsRedirection();

//使用身份認證
app.UseAuthentication();
//使用訪問授權
app.UseAuthorization();

app.MapControllers();

app.Run();
