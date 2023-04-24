using WebApiMaxine.Common;

namespace WebApiMaxine.Response
{
    public class AuthorizationResp
    {
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public String? User { get; set; }

        /// <summary>
        /// 登入狀態
        /// </summary>
        public ReturnCodes State { get; set; } = ReturnCodes.CODE_FAILURE;

        public String? challenge { get; set; } 

        public String? token { get; set; }

    }
}
