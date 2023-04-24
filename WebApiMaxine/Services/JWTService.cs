using WebApiMaxine.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiMaxine.Repository.Contexts;

namespace WebApiMaxine.Services;

public class JWTService : IJWTService
{
    private readonly IConfiguration _config;
    private readonly DBContext _dbContext;

    public JWTService(IConfiguration configuration, DBContext dbContext)
    {
        _config = configuration;
        _dbContext = dbContext;
    }

    /// <summary>
    /// 產生Token
    /// </summary>
    /// <param name="account">使用者帳號</param>
    /// <returns>Token</returns>
    public string CreatToken(string account)
    {
        var now = DateTime.UtcNow;
        
        var claims = new Claim[]
        {                
            new Claim(JwtRegisteredClaimNames.Sub, account),                
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),                
            new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
        };            
        
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtToken:Secret"]));

        //取得資料庫Token過期時間
        //var tokenExpires = int.Parse(_dbContext.Settings.Where(x => x.SettingKey == "TokenExpires").Single().SettingValue);

        var jwt = new JwtSecurityToken(                
            issuer: _config["JwtToken:Issuer"],                
            audience: _config["JwtToken:Audience"],                
            claims: claims,                
            notBefore: now,                
            expires: now.Add(TimeSpan.FromMinutes(20)),  //過期時間            
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
         );
        //給定 jwt 設定檔，產生並返回新組新的 token 雜湊
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
}
