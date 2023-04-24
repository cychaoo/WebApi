using System.Security.Cryptography.X509Certificates;
using WebApiMaxine.Common;
using WebApiMaxine.Request;
using WebApiMaxine.Response;

namespace WebApiMaxine.Interface
{
    public interface IX509CertService
    {
        public X509Certificate2 CreateCertificate(CertificateReq req);
        public ResponseResult<CertificateResp> VerifyCert(X509Certificate2 cert);

        public ResponseResult<CertificateResp> StoredCert(X509Certificate2 cert, CertificateReq req);

        public bool ExportCertificate(CertificateReq req, string exportFile);
    }
}
