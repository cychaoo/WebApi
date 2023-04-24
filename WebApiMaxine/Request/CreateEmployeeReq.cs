using System.ComponentModel.DataAnnotations;


namespace WebApiMaxine.Requset;

public class CreateEmployeeReq
{
    [Required]
    [RegularExpression(@"^[sS][0-9]{5}|[mM][0-9]{5}$", ErrorMessage = "帳號不符合格式")]
    [StringLength(6)]
    public string? UserId { get; set; }

    [Required]
    [StringLength(20)]
    public string? Name { get; set; }
}
