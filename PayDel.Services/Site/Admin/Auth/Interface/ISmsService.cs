using System;
using System.Collections.Generic;
using System.Text;

namespace PayDel.Services.Site.Admin.Auth.Interface
{
    public interface ISmsService
    {
        #region Auth
        bool SendFastVerificationCode(string mobile, string code);
        bool SendVerificationCode(string mobile, string code);
        #endregion


        string GetLineNumber();
    }
}
