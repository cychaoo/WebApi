using WebApiMaxine.DTO;
using WebApiMaxine.Enums;
using WebApiMaxine.ErrorHandling;
using WebApiMaxine.Interface;
using WebApiMaxine.Response;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiMaxine.Repository.Contexts;
using WebApiMaxine.Repository.Models;

namespace WebApiMaxine.Services;

public class EmployeeService : IEmployeeService
{
    private readonly DBContext _dbContext;
    private string _emTotalCount = "0";

    public EmployeeService(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 取得所有員工資訊
    /// </summary>
    /// <returns></returns>
    public async Task<EmployeeListResp> GetEmployeeListAsync()
    {

        var employeeList = await GetEmListAsync();
        var newEmployeeList = new List<EmployeeDTO>((IEnumerable<EmployeeDTO>)employeeList);
        //id去空白
        foreach (var item in newEmployeeList)
        {
            item.UserId = item.UserId.Trim();
        }

        return new EmployeeListResp { TotalCount = _emTotalCount, EmList = newEmployeeList };
    }

    /// <summary>
    /// 取得單一員工資訊
    /// </summary>
    /// <returns></returns>
    public async Task<EmployeeDTO> GetEmployeeAsync(string id)
    {
        var employee = await _dbContext.Employees
            .Where(x => x.Id == id)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (employee == null)
            throw new BusinessException(BusinessError.NoData, "ID no found");

        EmployeeDTO employeeMap = new EmployeeDTO() { UserId = employee.UserId.Trim() };

        return employeeMap;
    }



    /// <summary>
    /// 新增員工資訊
    /// </summary>
    /// <returns></returns>
    public async Task InsertEmployeeAsync(EmployeeDTO request)
    {
        var employee = _dbContext.Employees
            .Where(x => x.UserId == request.UserId)
            .AsNoTracking()
            .SingleOrDefault();
        if (employee == null)
            throw new BusinessException(BusinessError.NoData, "UserId no found!");

        var newEmployee = new Employee()
        {
            UserId = request.UserId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            Salary = request.Salary
        };

        _dbContext.Employees.Update(newEmployee);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 更新員工資訊
    /// </summary>
    /// <returns></returns>
    /// 
    public async Task UpdateEmployeeAsync(EmployeeDTO request)
    {
        var employee = _dbContext.Employees
            .Where(x => x.UserId == request.UserId)
            .AsNoTracking()
            .SingleOrDefault();

        if (employee == null)
            throw new BusinessException(BusinessError.NoData, "ID no found");

        var newEmployee = new Employee()
        {
            UserId = request.UserId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            Salary = request.Salary
        };

        _dbContext.Employees.Update(newEmployee);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 刪除員工資訊
    /// </summary>
    /// <returns></returns>
    public async Task DeleteEmployeeAsync(string id, EmployeeDTO request)
    {
        var employee = await _dbContext.Employees
            .Where(x => x.Id == id)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        var user = await _dbContext.Users
            .Where(x => x.UserId == employee!.UserId)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (employee == null)
            throw new BusinessException(BusinessError.NoData, "ID no found");

        var newEmployee = new Employee()
        {
            UserId = "resign",
            FirstName = null,
            LastName = null,
            Gender = null,
            Salary = 0
        };

        _dbContext.Employees.Update(newEmployee);

        _dbContext.Users.Remove(user!);

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 依Id及帳號取得員工清單
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    private async Task<List<Employee>> GetEmListAsync()
    {
        var employeeList = new List<Employee>();

        //取得全部
        employeeList = await _dbContext.Employees
            .OrderByDescending(x => x.Id)
            .AsNoTracking()
            .ToListAsync();

        //總筆數
        _emTotalCount = employeeList.Count().ToString();

        return employeeList;
    }
}
