using WebApiMaxine.Common;
using WebApiMaxine.Request;
using WebApiMaxine.Response;

namespace WebApiMaxine.Interface
{
    public interface IQREncodeService
    {
        public ResponseResult<QREncodeResp> GenQRCode(QREncodeReq req);
    }
}
