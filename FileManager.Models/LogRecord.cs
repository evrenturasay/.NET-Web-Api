using System;

namespace FileManager.Models
{
    // LogRecord sınıfı, dosya yükleme ve indirme işlemleri sırasında oluşan log kayıtlarını temsil eder.
    public class LogRecord
    {
        public DateTime LogDate { get; set; }
        public string FileName { get; set; }
        public string State { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }

    }
}