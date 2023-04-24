using WebApiMaxine.DTO;
using WebApiMaxine.Repository.Models;
using WebApiMaxine.Requset;
using WebApiMaxine.Response;
namespace WebApiMaxine.Interface;

public interface IEmployeeService
{


    Task<EmployeeListResp> GetEmployeeListAsync();

    Task<EmployeeDTO> GetEmployeeAsync(string id);

    Task InsertEmployeeAsync(EmployeeDTO request);

    Task UpdateEmployeeAsync(EmployeeDTO request);

    Task DeleteEmployeeAsync(string id, EmployeeDTO request);
  
}

