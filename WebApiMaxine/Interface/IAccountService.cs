using WebApiMaxine.Requset;
using WebApiMaxine.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMaxine.Interface;

public interface IAccountService
{
    Task<LoginResp> LoginAsync(AuthenticateModel requset);

    Task InsertUserAsync(CreateUserReq request);
}
