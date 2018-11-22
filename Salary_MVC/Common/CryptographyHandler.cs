using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Common
{
    /// <summary>
    /// 加密、解密处理,
    /// </summary>
    public class CryptographyHandler
    {
        static readonly string mode = System.Configuration.ConfigurationManager.AppSettings["Cryptography_Mode"];

        static readonly Password_Encrypt_ASC.Password_Encrypt_ASC PEA = new Password_Encrypt_ASC.Password_Encrypt_ASC();

        static Dictionary<string, Func<string, string>> dictSet = new Dictionary<string, Func<string, string>>() { { "0",PEA.set_password_ASC} , { "1",blank.Password_Encrypt_ASC.set_password_ASC} };

        static Dictionary<string, Func<string, string>> dictGet = new Dictionary<string, Func<string, string>>() { { "0", PEA.get_password_ASC }, { "1", blank.Password_Encrypt_ASC.get_password_ASC } };

        static readonly Func<string, string> setHandler = dictSet[mode];

        static readonly Func<string, string> getHandler = dictGet[mode];

        public string set_password_ASC(string str_value)
        {
            return setHandler(str_value);
        }

        public string get_password_ASC(string v)
        {
            return getHandler(v);
        }
    }
}