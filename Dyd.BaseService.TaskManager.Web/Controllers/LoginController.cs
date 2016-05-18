using Dyd.BaseService.TaskManager.Domain.Dal;
using Dyd.BaseService.TaskManager.Domain.Model;
using Dyd.BaseService.TaskManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using XXF.BasicService.CertCenter;

namespace Dyd.BaseService.TaskManager.Web.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        //登录
        [HttpGet]
        public ActionResult Login(string appid, string sign, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }   
    
        //登录
        [HttpPost]
        public ActionResult Login(string appid, string sign, string returnUrl, string username, string password, string validate)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["Admin"].Contains(";" + username + "," + password + ";"))
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(username, false, (int)FormsAuthentication.Timeout.TotalMinutes);
                string enticket = FormsAuthentication.Encrypt(ticket);
                HttpCookie auth = new HttpCookie(FormsAuthentication.FormsCookieName, enticket);
                Response.AppendCookie(auth);
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Task");
                }
            }
            else
            {
                ModelState.AddModelError("", "登陆出错,请咨询管理员。");
                return View();
            }
        }

        //登出
        public ActionResult Logout(string returnurl)
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }
    }
}
