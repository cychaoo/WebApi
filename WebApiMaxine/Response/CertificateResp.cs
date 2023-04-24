using System.Security.Cryptography.X509Certificates;

namespace WebApiMaxine.Response
{
    public class CertificateResp
    {
        public String Subject { get; set; } = string.Empty;

        public bool Issuer { get; set; } 

        public String SerialNumber { get; set;} = string.Empty;

        public DateTime NotBefore { get; set; }

        public DateTime NotAfter { get; set; }

        public string Thumbprint { get; set; } = string.Empty;

        public X509Certificate2? Cert { get; set; }
    }
    
}
