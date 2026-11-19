#region

using log4net;
using log4net.Appender;

using Microsoft.AspNetCore.Mvc;

#endregion

namespace Sindvamb.HangFire.Host.Controllers
{
    [Route("error/log")]
    public class ErrorLoggingController
    {
        [HttpGet]
        public IActionResult Index()
        {
            var path = (LogManager.GetCurrentLoggers()[0].Logger.Repository.GetAppenders()[0] as FileAppender).File;
            var result = new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), "binary/octet-stream");
            result.FileDownloadName = "Logs.log";
            return result;
        }
    }
}