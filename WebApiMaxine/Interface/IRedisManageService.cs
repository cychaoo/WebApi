
using System.Diagnostics.Eventing.Reader;

namespace WebApiMaxine.Interface;

public interface IRedisManageService
{

    void Set(string key, object value, TimeSpan ts);
    string GetValue(string key);
    bool Delete (string key);
}
