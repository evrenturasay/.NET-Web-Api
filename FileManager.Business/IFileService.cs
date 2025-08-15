using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using FileManager.Models;

namespace FileManager.Business
{
    // Dosya işlemleri için interface tanımı
    public interface IFileService
    {
        //dosya işlemleri için gerekli metotların tanımlanması
        void UploadFile(HttpPostedFileBase uploadedFile);
        IEnumerable<FileDto> GetAllActiveFiles();
        FileDto GetFileById(Guid id);
        void SoftDeleteFile(Guid id);
        MemoryStream DownloadFile(string s3key);
    }
}
