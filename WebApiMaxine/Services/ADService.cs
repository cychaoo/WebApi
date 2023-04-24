using Microsoft.EntityFrameworkCore;
using WebApiMaxine.DTO;
using WebApiMaxine.Interface;
using WebApiMaxine.Requset;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApiMaxine.Repository.Contexts;

namespace WebApiMaxine.Services;

public class ADService : IADService
{
    private readonly DBContext _dbContext;

    public ADService(DBContext dbContext) {
        _dbContext = dbContext;
    }
    public bool ADLogin(AuthenticateModel queryData, string My_LDAP, string domain)
    {
        var port = My_LDAP.Split(':');
        var skb_LDAP = new LdapDirectoryIdentifier(port[0], Convert.ToInt32(port[1]));

        //驗證
        try
        {
            using (var ldapConnection = new LdapConnection(skb_LDAP))
            {
                Console.WriteLine("My_LDAP is created successfully.");
                ldapConnection.AuthType = AuthType.Basic;
                ldapConnection.SessionOptions.ProtocolVersion = 3;
                var nc = new NetworkCredential($"{queryData.UserId}@{domain}", queryData.UserWord);
                ldapConnection.Bind(nc);
            }
            Console.WriteLine(queryData.UserId + "-帳號密碼驗證完成");
            return true;
        }
        catch (Exception ex)
        {
            var extemp = ex;
            while (extemp != null)
            {
                Console.WriteLine(extemp.Message);
                extemp = ex.InnerException;
            }
            return false;
        }
    }

    /// <summary>
    /// 一般登入
    /// </summary>
    /// <param name="queryData"></param>
    /// <returns></returns>
    public bool Login(AuthenticateModel queryData)
    {
        //驗證

        var secret = Convert.ToBase64String(Encoding.UTF8.GetBytes(queryData.UserWord));
        var user = _dbContext.Users
               .Where(x => x.UserId == queryData.UserId & x.UserWord == secret)
               .AsNoTracking()
               .SingleOrDefaultAsync();

        if (user == null)
            return false;

        Console.WriteLine(queryData.UserId + "-帳號密碼驗證完成");
        return true;
        
    }
}
