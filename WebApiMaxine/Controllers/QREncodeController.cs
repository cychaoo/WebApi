using Microsoft.AspNetCore.Mvc;
using WebApiMaxine.Interface;
using WebApiMaxine.Request;

namespace WebApiMaxine.Controllers
{
    public class QREncodeController : Controller
    {
        private readonly IQREncodeService _qrEncodeService;
        public QREncodeController(IQREncodeService qrEncodeService)
        {
            _qrEncodeService = qrEncodeService;
        }
        /// <summary>
        /// 產生編碼網址
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoGenQRCode([FromBody] QREncodeReq req)
        {
            var result = _qrEncodeService.GenQRCode(req);
            // 回傳 Json 給前端
            return Json(result);
        }
    }
}
