using System;

namespace FileManager.Models
{
    // Dosya bilgilerini tutan veri modeli
    public class FileRecord
    {
        // Dosya kaydı için gerekli alanlar
        public Guid Id { get; set; } //Dosya id
        public string FileName { get; set; }   // Dosya adı  
        public string S3Key { get; set; }  // S3 üzerinde dosyanın anahtarı
        public DateTime UploadDate { get; set; } // Yükleme tarihi
        public bool IsDeleted { get; set; } // Soft delete durumu
        public string FileType { get; set; } // Dosya tipi 
    }
}
