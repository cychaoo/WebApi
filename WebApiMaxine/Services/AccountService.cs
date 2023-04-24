using WebApiMaxine.Enums;
using WebApiMaxine.Interface;
using WebApiMaxine.Repository.Models;
using WebApiMaxine.Requset;
using WebApiMaxine.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text;
using WebApiMaxine.Repository.Contexts;
using WebApiMaxine.ErrorHandling;

namespace WebApiMaxine.Services;

public class AccountService : IAccountService
{
    private readonly DBContext _dbContext;        
    private readonly IJWTService _jwtService;
    private readonly IADService _adService;

    public AccountService(DBContext dbContext, IJWTService jwtService, IADService adService)
    {
        _dbContext = dbContext;            
        _jwtService = jwtService;
        _adService = adService;
        
    }

    public async Task<LoginResp> LoginAsync(AuthenticateModel requset)
    {

        //判斷帳號是否有在User資料表內,有才能登入
        var getUser = await GetUser(requset);

        if (!getUser)
            throw new BusinessException(BusinessError.LoginFailed, "無此使用者");

        //var loginSuccess = _adService.ADLogin(requset, ldap, doamin);
        var loginSuccess = _adService.Login(requset);

        if (!loginSuccess)
            //登入失敗
            throw new BusinessException(BusinessError.LoginFailed, "登入失敗");


        //更新會員資料表
        await UpdateUser(requset.UserId);

        //產生Token
        var token = _jwtService.CreatToken(requset.UserId);

        var user = await _dbContext.Users
                .Where(x => x.UserId == requset.UserId)
                .AsNoTracking()
                .SingleOrDefaultAsync();

        return new LoginResp
        {
            UserId = requset.UserId,
            UserName = user?.UserName,
            Token = token
        };
    }

    /// <summary>
    /// 更新會員資料表
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private async Task<User> UpdateUser(string? userId)
    {
        var user = await _dbContext.Users
            .Where(x => x.UserId == userId)
            .AsNoTracking()
            .SingleAsync();

        //更新會員最新登入日期
        user.LastLoginDate = DateTime.Now;
        _dbContext.Users.Update(user);

        await _dbContext.SaveChangesAsync();

        return user;
    }

    /// <summary>
    /// 註冊(新增)會員資料表
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// 
    public async Task InsertUserAsync(CreateUserReq request)
    {
        var entity = await _dbContext.Users
            .Where(x => x.UserId == request.UserId)
             .AsNoTracking()
            .SingleOrDefaultAsync();

        var employeeInfo = await _dbContext.Employees
            .Where(x => x.UserId == request.UserId)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        var employeeLast = _dbContext.Employees
           .OrderBy(x => x.Id)
           .AsNoTracking()
           .LastOrDefault(); //取最後一筆

        if (entity != null)
        {
            throw new BusinessException(BusinessError.UserIdIsExist, "userId is exist");
        }

        var newUser = new User()
        {
            UserId = request.UserId,
            UserName = request.UserName,
            CreateDate = DateTime.Now,
            UserWord = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.UserWord))
        };

        var newEmployee = new Employee()
        {
            //取得新增資料後回寫Id
            Id = (int.Parse(employeeLast!.Id) + 1).ToString().Trim(),
            UserId = request.UserId,
            FirstName = request.UserName
        };

    
        await _dbContext.Users.AddAsync(newUser);
        await _dbContext.Employees.AddAsync(newEmployee);
        await _dbContext.SaveChangesAsync();

    }
    private async Task<bool> GetUser(AuthenticateModel requset)
    {
        var user = await _dbContext.Users
                .Where(x => x.UserId == requset.UserId)
                .AsNoTracking()
                .SingleOrDefaultAsync();

        if (user == null)
            return false;

        return true;
    }
}
