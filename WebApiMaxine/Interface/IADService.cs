using WebApiMaxine.DTO;
using WebApiMaxine.Requset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMaxine.Interface;

public interface IADService
{
    bool ADLogin(AuthenticateModel queryData, string My_LDAP, string domain);

    bool Login(AuthenticateModel queryData);
}
