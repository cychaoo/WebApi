using System.ComponentModel.DataAnnotations;


namespace WebApiMaxine.Requset;

public class DeleteEmployeeReq
{
    [Required]
    [RegularExpression(@"^[sS][0-9]{5}|[mM][0-9]{5}$", ErrorMessage = "帳號不符合格式")]
    public string ?UserId { get; set; }
}
