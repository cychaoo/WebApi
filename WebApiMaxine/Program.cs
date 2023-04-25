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

var env = builder.Environment.EnvironmentName; //�����ҦW��
var aesKey = builder.Configuration.GetValue<string>("AES_KEY") ?? throw new ArgumentNullException("AES_KEY");
var ivKey = builder.Configuration.GetValue<string>("IV_KEY") ?? throw new ArgumentNullException("IV_KEY"); // [�s�W]ivKey��json���ȡAbyMaxine

// �z�L builder.Services �N�A�ȥ[�J DI �e��
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(AuthorizationFilter)); // �s�W���ұ��v
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
SystemShared.Init(); //��l�Ƴ]�w
//�榡��SQL�s�u��T�r��
var ConnectionString = SystemShared.BuildConnectionString(databaseConfigDetails);  //[2022/12/02 �վ�]: �[�J�ѼơA�@�γs�u��T
//TODO:Added by Roger:���USession�\��
builder.Services.AddSession();
//�s�WEF Core��Ʈw�A��
builder.Services.AddDbContext<DBContext>(option => option.UseSqlServer(ConnectionString));
builder.Services.AddTransient<DBContext>();

builder.Services.Configure<RedisConfigDTO>(builder.Configuration.GetSection("Redis"));
builder.Services.AddDiContainer();

#region SimpleCache �ϧ����ҽX
builder.Services.AddMemoryCache()
.AddSimpleCaptcha(builder =>
{
    builder.UseMemoryStore();
});
#endregion

#region JWT Token����
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
    x.RequireHttpsMetadata = false;//���o�γ]�w���~��Ʀ�}�α��v���O�_�ݭn HTTPS�C �w�]�ȬO true�C �o���ӥu�b�}�o���Ҥ����ΡC
    x.TokenValidationParameters = tokenValidationParameters;//�]�w���Ҫ��Ѽ�
    x.Events = new JwtBearerEvents
    {
        //Token�L��
        OnAuthenticationFailed = context =>
        {
            context.Response.StatusCode = 401;
            context.HttpContext.Response.WriteAsync("Lifetime validation failed. The token is expired.");

            return Task.CompletedTask;
        },
        //�v�����ҥ��ѫ�Ĳ�o���ƥ�
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
app.UseHttpLogging(); //�i����O��Http�n�D/�^�����e

app.UseHttpsRedirection();

//�ϥΨ����{��
app.UseAuthentication();
//�ϥγX�ݱ��v
app.UseAuthorization();

app.MapControllers();

app.Run();
