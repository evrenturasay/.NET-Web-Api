using System;
using System.Collections.Generic;
using FileManager.Models;

namespace FileManager.Data
{
    // Dosya işlemleri için repository İnterface
    public interface IFileRepository
    {
        // Dosya işlemleri için gerekli metotların tanımlanması
        void AddFile(FileRecord file);
        IEnumerable<FileRecord> GetAllFiles();
        FileRecord GetFileById(Guid id);
        void SoftDeleteFile(Guid id);
    }
}
