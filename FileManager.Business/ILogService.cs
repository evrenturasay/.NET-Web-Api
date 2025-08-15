using FileManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FileManager.Business
{
    //Loglama servisi için İnterface
    public interface ILogService
    {
        void LogError(string name, string message, Exception ex);
        void LogInfo(string name, string message);
        IEnumerable<LogRecord> GetLogs();
    }
}

