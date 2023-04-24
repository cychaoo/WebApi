using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiMaxine.Repository.Contexts;
using WebApiMaxine.Common;
using System.Collections.Generic;
using WebApiMaxine.Interface;
using WebApiMaxine.Response;
using WebApiMaxine.Requset;
using WebApiMaxine.DTO;
using Microsoft.AspNetCore.Authorization;


namespace WebApiMaxine.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly DBContext _dbContext;
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService, DBContext dbContext)
    {
        _dbContext = dbContext;
        _employeeService = employeeService;
    }

    /// <summary>
    /// 讀取所有員工
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetEmployeeList")]
    public async Task<ResponseResult<EmployeeListResp>> GetEmployeeList()
    {
        var content = await _employeeService.GetEmployeeListAsync();

        return new ResponseResult<EmployeeListResp>() { RtnCode = (int)ReturnCodes.CODE_SUCCESS, Msg = MsgCodes.Msg_00, Data = content };
    }

    /// <summary>
    /// 讀取單一員工
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ResponseResult<EmployeeDTO>> Get(string id)
    {
        var content = await _employeeService.GetEmployeeAsync(id);

        return new ResponseResult<EmployeeDTO>() { RtnCode = (int)ReturnCodes.CODE_SUCCESS, Msg = MsgCodes.Msg_00, Data = content };
    }

    /// <summary>
    /// 新增員工資訊
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    //[HttpPost("CreateOrUpdateEmployee")]
    //public async Task<ResponseResult<EmployeeDTO>> CreateOrUpdateEmployee(EmployeeDTO request)
    //{
    //    await _employeeService.InsertEmployeeAsync(request);

    //    return new ResponseResult<EmployeeDTO>() { RtnCode = ReturnCodes.CODE_SUCCESS, Msg = MsgCodes.Msg_00 };
    //}

    /// <summary>
    /// 更新員工資訊
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("UpdateEmployeeInfo")]
    public async Task<ResponseResult<EmployeeDTO>> UpdateEmployee(EmployeeDTO request)
    {
        await _employeeService.UpdateEmployeeAsync(request);

        return new ResponseResult<EmployeeDTO>() { RtnCode = (int)ReturnCodes.CODE_SUCCESS, Msg = MsgCodes.Msg_00 };
    }

    /// <summary>
    /// 刪除員工(離職)
    /// </summary>
    /// <returns></returns>
    [HttpPut("Delete/{id}")]
    public async Task<ResponseResult<EmployeeListResp>> Delete(string id, EmployeeDTO request)
    {
        await _employeeService.DeleteEmployeeAsync(id,request);

        return new ResponseResult<EmployeeListResp>() { RtnCode = (int)ReturnCodes.CODE_SUCCESS, Msg = MsgCodes.Msg_00 };
    }

}
