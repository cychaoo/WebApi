using System.Drawing;
using System.Drawing.Imaging;
using WebApiMaxine.Common;
using WebApiMaxine.Interface;
using WebApiMaxine.Request;
using WebApiMaxine.Response;
using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace WebApiMaxine.Services
{
    public class QREncodeService : IQREncodeService
    {
        public QREncodeService() { }

        public ResponseResult<QREncodeResp> GenQRCode(QREncodeReq req)
        {
            //檢查來源
            if (string.IsNullOrEmpty(req.UrlEnText))
                return new ResponseResult<QREncodeResp>() { RtnCode = 999, Msg = "請輸入已編碼網址" };
            //Barcode物件
            var bw = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new QrCodeEncodingOptions { Height =300, Width=300}
            };

            //產生QRcode
            var img = bw.Write(req.UrlEnText); //來源
            Bitmap bm = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppRgb);
            string? dir = Path.GetDirectoryName(req.FileName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);

            bm.Save(req.FileName, ImageFormat.Gif);
            bm.Dispose();
            return new ResponseResult<QREncodeResp>() { RtnCode = 0, Msg = "成功" };
        }
    }
}
