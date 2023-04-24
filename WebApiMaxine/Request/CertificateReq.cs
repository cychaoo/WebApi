using System.Security.Cryptography.X509Certificates;

namespace WebApiMaxine.Request
{
    public class CertificateReq
    {
        /// <summary>
        /// The path to the certificate.
        /// </summary>
        public String Path { get; set; } = string.Empty;

        public String FileName { get; set; } = string.Empty;

        public String Password { get; set; } = string.Empty;

        public String Subject { get; set; } = String.Empty;

        public String StoreName { get; set; } = string.Empty;

        public String StoreLocation { get; set;} = string.Empty;

        public String ContentType { get; set;}    = string.Empty;
    }
}
