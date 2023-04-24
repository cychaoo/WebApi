using System.Collections.Generic;
using WebApiMaxine.DTO;

namespace WebApiMaxine.Response;

public class EmployeeListResp
{
    public string? TotalCount { get; set; }

    public List<EmployeeDTO>? EmList { get; set; }

}
