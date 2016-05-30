using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TaskManager.Web.Models
{
    public class AuthorityCheck : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                var userlogininfo = UserLoginInfo.CurrentUserLoginInfo;
                if (userlogininfo != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception exp)
            {
                return false;
            }
        }
    }
}