using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using WebApiMaxine.Common;
using WebApiMaxine.Interface;
using WebApiMaxine.Repository.Models;
using WebApiMaxine.Request;
using WebApiMaxine.Response;
using WebApiMaxine.Services;

namespace WebApiMaxine.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class X509CertificateController : ControllerBase
{
    private readonly IX509CertService _x509CertService;
    public X509CertificateController(IX509CertService x509CertService)
    {
        _x509CertService = x509CertService;
    }

    /// <summary>
    /// 讀取憑證
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("CertificateInfo")]
    public ResponseResult<CertificateResp> LoadPFXFile([FromBody] CertificateReq req)
    {
        // Load the certificate into an X509Certificate object.
        X509Certificate2 cert = _x509CertService.CreateCertificate(req);

        //檢查憑證有效日
        var result = _x509CertService.VerifyCert(cert);

        return new ResponseResult<CertificateResp>() { RtnCode = result.RtnCode, Msg = result.Msg, Data = result.Data };
    }

    /// <summary>
    /// 匯入儲存憑證
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("ImportCertificate")]
    public ResponseResult<CertificateResp> ImportCertificate([FromBody] CertificateReq req)
    {
        // Load the certificate into an X509Certificate object.
        X509Certificate2 cert = _x509CertService.CreateCertificate(req);

        //儲存憑證
        var result = _x509CertService.StoredCert(cert, req);
        return new ResponseResult<CertificateResp>() { RtnCode = result.RtnCode, Msg = result.Msg };
    }

    /// <summary>
    /// 匯入儲存憑證
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("ExportCertificate")]
    public bool ExportCertificate([FromBody] CertificateReq req)
    {
        string exportFile = "";
        return _x509CertService.ExportCertificate(req, exportFile);
    }
}
