using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiMaxine.Interface;

public interface IJWTService
{
    public string CreatToken(string account);
}
