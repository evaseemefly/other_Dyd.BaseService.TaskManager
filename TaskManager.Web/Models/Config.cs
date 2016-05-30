using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManager.Web.Models
{
    public class Config
    {
        public static string TaskConnectString = System.Configuration.ConfigurationManager.AppSettings["TaskConnectString"].ToString();
    }
}