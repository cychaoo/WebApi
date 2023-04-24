using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMaxine.DTO;

public class EmployeeDTO
{

    //[StringLength(50)]
    //public string Id { get; set; }

    [StringLength(50)]
    public string UserId { get; set; } = null!;


    [StringLength(50)]
    public string ?FirstName { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    [StringLength(50)]
    public string ?Gender { get; set; }

    [Range(0,99999)]
    public int Salary { get; set; }

   
}
