using FileManager.AWS;
using FileManager.Data;
using FileManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Business
{
    public class LogService : ILogService
    {
        // Loglama servisi için Logger sınıfı
        private readonly ILogger _logger;

        public LogService()
        {
            // Logger sınıfının örneğinin oluşturulması
            _logger = new Logger();
        }
        public void LogError(string name, string message, Exception ex)// Loglama işlemi için hata mesajı
        {
            _logger.LogError(name, message, ex);
        }
        public void LogInfo(string name, string message)// Loglama işlemi için bilgi mesajı
        {
            _logger.LogInfo(name, message);
        }
        public IEnumerable<LogRecord> GetLogs()
        {
            var records = _logger.GetAllLogs();

            return records.Select(l => new LogRecord
            {
                LogDate = l.LogDate,
                FileName = l.FileName,
                State = l.State,
                Message = l.Message,
                Exception = l.Exception
            });
        }

    }
}
