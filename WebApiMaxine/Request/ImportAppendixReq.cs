using System.IO;
using System.Web;

namespace WebApiMaxine.Requset
{
    public class ImportAppendixReq
    {
        public Guid UserId { get; set; }
        /// <summary>
        /// 附件
        /// </summary>


        public ICollection<IFormFile> Files { get; set; }

    }

    public class AppendFile
    {
        public Guid FileId { get; set; }
        public string? FileName { get; set; }
        public string? Extension { get; internal set; }
    }
}
