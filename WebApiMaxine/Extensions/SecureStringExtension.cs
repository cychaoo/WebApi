using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WebApiMaxine.Extensions
{
    /// <summary>
    /// Author:Roger Date:2022/11/30
    /// </summary>
    ///<remarks>加密字串擴充類別</remarks>
    static public class SecureStringExtension
    {
        /// <summary>
        /// 取得加密字串(密碼)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static private SecureString getPwdSecurity(string value)
        {
            SecureString result = new SecureString();
            foreach (char c in value)
            {
                result.AppendChar(c);
            }

            return result;
        }

        /// <summary>
        /// 將字元陣列編碼為加密字串
        /// </summary>
        /// <param name="self">字元陣列</param>
        /// <returns></returns>
        static public SecureString ToSecureString(this char[] self)
        {
            var secureString = new SecureString();
            foreach (char c in self)
            {
                secureString.AppendChar(c);
            }
            return secureString;
        }

        /// <summary>
        /// 將一般字串編碼為加密字串
        /// </summary>
        /// <param name="self">字串</param>
        /// <returns></returns>
        static public SecureString ToSecureString(this string self)
        {
            var secureString = new SecureString();
            char[] chars = self.ToCharArray();
            foreach (char c in chars)
            {
                secureString.AppendChar(c);
            }
            return secureString;
        }

        /// <summary>
        /// 將加密字串解碼為一般字串
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        static public string ToText(this SecureString self)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(self);
            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }
    }
}
