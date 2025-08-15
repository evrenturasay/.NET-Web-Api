using FileManager.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApi.Controllers
{
    public class LogController : Controller
    {
        // Loglama işlemleri için servis
        private readonly ILogService _logService;
        public LogController()
        {
            // Loglama servisi için LogService sınıfının örneğinin oluşturulması
            _logService = new LogService();
        }

        public ActionResult Index()// Log kayıtlarının listelendiği sayfa
        {
            var logs = _logService.GetLogs();
            return View(logs);
        }
    }
}