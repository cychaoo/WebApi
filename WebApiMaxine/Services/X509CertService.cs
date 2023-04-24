using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using WebApiMaxine.Common;
using WebApiMaxine.Interface;
using WebApiMaxine.Request;
using WebApiMaxine.Requset;
using WebApiMaxine.Response;

namespace WebApiMaxine.Services
{
    public class X509CertService : IX509CertService
    {
        private readonly X509ContentType _contentType = X509ContentType.Cert;
        public X509CertService()
        {
        }
        /// <summary>
        /// 建立憑證
        /// </summary>
        /// <param name="certFile">憑證檔案</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public X509Certificate2 CreateCertificate(CertificateReq req)
        {
            // The path to the certificate.
            string certFile = req.Path + req.FileName;

            if (certFile == null) 
                throw new ArgumentNullException("CertFile");
            if (req.Password == null) 
                throw new ArgumentNullException("Password");
            X509Certificate2 cert;
            if (string.IsNullOrEmpty(req.Password))
            {
                cert = new X509Certificate2(certFile);
            }
            else
            {
                cert = new X509Certificate2(certFile, req.Password);
            }
            return cert;
        }
        /// <summary>
        /// 檢查憑證效期
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResponseResult<CertificateResp> VerifyCert(X509Certificate2 cert)
        {
            try
            {
                if (cert == null)
                    return new ResponseResult<CertificateResp>() { RtnCode = 999, Msg = "Cert is null" };

                X509Chain chain = new();
                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                chain.Build(cert);

                if (cert.NotAfter <= DateTime.Now)
                    return new ResponseResult<CertificateResp>() { RtnCode = 999, Msg = "憑証過期" };

                return new ResponseResult<CertificateResp>() { RtnCode = 0, Msg = "succuss" };
            }
            catch (Exception ex)
            {
                return new ResponseResult<CertificateResp>() { RtnCode=999, Msg= ex.Message };
            }
        }
        /// <summary>
        /// 匯入和儲存憑證
        /// </summary>
        /// <param name="cert"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public ResponseResult<CertificateResp> StoredCert(X509Certificate2 cert, CertificateReq req)
        {
            X509Store store = new(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            store.Add(cert);
            store.Close();
            var result = new CertificateResp() { Cert= cert };
            return new ResponseResult<CertificateResp>() { RtnCode = 0, Msg = "succuss", Data = result };
        }
        /// <summary>
        /// 匯出憑證
        /// </summary>
        /// <param name="req"></param>
        /// <param name="exportFile"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool ExportCertificate(CertificateReq req, string exportFile)
        {
            if (exportFile == null) throw new ArgumentNullException("ExportFile");
            X509Store store = new(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            FileStream? fileStream = null;
            try
            {
                fileStream = new FileStream(exportFile, FileMode.Create, FileAccess.Write);
                foreach (X509Certificate2 cert in store.Certificates)
                {
                    if (cert.Subject == req.Subject)
                    {
                        byte[] CertByte = string.IsNullOrEmpty(req.Password) ? cert.Export(X509ContentType.Cert) : cert.Export(X509ContentType.Cert, req.Password);
                        fileStream.Write(CertByte, 0, CertByte.Length);
                        return true;
                    }
                }
            }
            finally
            {
                fileStream?.Dispose();
                store.Close();
            }
            return false;
        }
    }
}
