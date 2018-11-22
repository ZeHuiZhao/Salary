using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Salary_MVC
{
    public partial class Default : System.Web.UI.Page
    {

        protected ValueWrapper InputWrapper { set; get; }
        protected override void OnLoad(EventArgs e)
        {
            this.InputWrapper = new ValueWrapper();
            InputWrapper.InputGuid= this.Request["InputGuid"];

            InputWrapper.InputJiaMi = this.Request["InputJiaMi"];

            if (!string.IsNullOrEmpty(InputWrapper.InputGuid))
                InputWrapper.JiaMiResult = this.Server.UrlEncode(blank.Password_Encrypt_ASC.set_password_ASC(InputWrapper.InputGuid));
            if (!string.IsNullOrEmpty(InputWrapper.InputJiaMi))
                InputWrapper.JieMiResult = blank.Password_Encrypt_ASC.get_password_ASC(this.Server.UrlDecode(InputWrapper.InputJiaMi));

        }
    }


    public class ValueWrapper
    {
        public string InputGuid { get; set; }
        public string JiaMiResult { get; set; }

        public string InputJiaMi { get; set; }

        public string JieMiResult { get; set; }
    }
}