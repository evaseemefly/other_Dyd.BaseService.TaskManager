using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core;
using TaskManager.Node;
using BSF.Api;

namespace TaskManager.WinService
{
    public class NodeService : ServiceBase
    {

        public void Start()
        {
            OnStart(null);
        }
        protected override void OnStart(string[] args)
        {
            try
            {
                if (System.Configuration.ConfigurationSettings.AppSettings.AllKeys.Contains("NodeID"))
                {
                    GlobalConfig.NodeID = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["NodeID"]);
                }
                if (string.IsNullOrWhiteSpace(GlobalConfig.TaskDataBaseConnectString) || GlobalConfig.NodeID <= 0)
                {
                    string url = GlobalConfig.TaskManagerWebUrl.TrimEnd('/') + "/OpenApi/" + "GetNodeConfigInfo/";
                    var r = ApiHelper.Post<dynamic>(url, new
                    {

                    });
                    if (r.success == false)
                    {
                        throw new Exception("请求" + url + "失败,请检查配置中“任务调度平台站点url”配置项");
                    }

                    dynamic appconfiginfo = r.data;
                    string connectstring = appconfiginfo["TaskDataBaseConnectString"];
                    connectstring = StringDESHelper.DecryptDES(connectstring, "dyd88888888");

                    if (string.IsNullOrWhiteSpace(GlobalConfig.TaskDataBaseConnectString))
                        GlobalConfig.TaskDataBaseConnectString = connectstring;
                    if (GlobalConfig.NodeID <= 0)
                        GlobalConfig.NodeID = appconfiginfo["NodeID"];
                }

                BSF.Tool.IOHelper.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\" + GlobalConfig.TaskSharedDllsDir + @"\");
                CommandQueueProcessor.Run();

                //注册后台监控
                GlobalConfig.Monitors.Add(new TaskManager.Node.SystemMonitor.TaskRecoverMonitor());
                GlobalConfig.Monitors.Add(new TaskManager.Node.SystemMonitor.TaskPerformanceMonitor());
                GlobalConfig.Monitors.Add(new TaskManager.Node.SystemMonitor.NodeHeartBeatMonitor());
                GlobalConfig.Monitors.Add(new TaskManager.Node.SystemMonitor.TaskStopMonitor());

                Node.Tools.LogHelper.AddNodeLog("节点windows服务启动成功");
            }
            catch (Exception exp)
            {
                Node.Tools.LogHelper.AddNodeError("节点windows服务启动失败", exp);
            }
        }

        protected override void OnStop()
        {
            Node.Tools.LogHelper.AddNodeLog("节点windows服务停止");
        }
    }
}
