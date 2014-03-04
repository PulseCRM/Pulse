using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace LPWeb.Common
{
    public class Encrypter
    {
        /// <summary>
        /// MD5加密（不可逆）
        /// 刘洋 2010-02-28
        /// </summary>
        /// <param name="sSource"></param>
        /// <returns></returns>
        public static string MD5Encrpty(string sSource)
        {
            MD5 md5 = MD5.Create();
            byte[] PwdBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(sSource));
            return BitConverter.ToString(PwdBytes);
        }

        #region Encode/Decode Base64
        /// <summary>
        /// Base64编码 UTF-8
        /// </summary>
        /// <param name="sCode"></param>
        /// <returns></returns>
        public static string Base64Encode(string sCode)
        {
            string encode = string.Empty;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(sCode);
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = sCode;
            }
            return encode;
        }

        /// <summary>
        /// Base64解码 UTF-8
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Base64Decode(string code)
        {
            string decode = string.Empty;
            try
            {
                byte[] bytes = Convert.FromBase64String(code);
                decode = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="code_type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Base64Encode(string code_type, string code)
        {
            string encode = string.Empty;
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="code_type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Base64Decode(string code_type, string code)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
        #endregion
    }
}