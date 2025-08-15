using System;

namespace FileManager.Models
{
    // Dosya bilgilerini tutan veri transfer nesnesi
    public class FileDto
    {
        // Dosya bilgilerinin tanımlanması
        public Guid Id { get; set; } // Dosya id
        public string FileName { get; set; } // Dosya Adı
        public string S3Key { get; set; } // Dosyanın S3 Key i
        public DateTime UploadDate { get; set; } // Dosyanın yüklenme tarihi


    }
}
