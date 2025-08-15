using FileManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Data
{
    // Loglama işlemleri için interface
    public interface ILogger
    {
        // Loglama işlemleri için metotlar
        void LogError(string name, string message, Exception ex);
        void LogInfo(string name, string message);
        IEnumerable<LogRecord> GetAllLogs();
    }
}
