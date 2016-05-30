%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe D:\svn-working\RT_Cloud\trunk\Ô´´úÂë\BaseService\TaskManager\TaskManager.WinService\bin\Debug\TaskManager.WinService.exe
Net Start TaskManager.WinService
sc config TaskManager.WinService start= auto
