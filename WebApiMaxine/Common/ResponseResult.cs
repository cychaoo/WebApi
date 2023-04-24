using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMaxine.Common;

public class ResponseResult<T>
{
    public int RtnCode { get; set; }

    public string ?Msg { get; set; }

    public T ?Data { get; set; }
}
