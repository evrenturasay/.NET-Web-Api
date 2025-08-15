using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileManager.Models;
using FileManager.Data;
using FileManager.AWS;


using System.Web;

namespace FileManager.Business
{
    public class FileService : IFileService
    {
        // Dosya işlemleri için repository ve S3 servisi
        private readonly IFileRepository _fileRepo;
        private readonly S3Service _s3Service;



        public FileService()
        {
            // Repository ve S3 servislerinin başlatılması
            _fileRepo = new FileRepository();
            _s3Service = new S3Service();
        }

        public void UploadFile(HttpPostedFileBase uploadedFile)// Dosya yükleme işlemi
        {
            // Dosya yükleme işlemi için gerekli değişkenlerin tanımlanması
            var random = new Random();
            var guid = Guid.NewGuid();
            var guid1 = Guid.NewGuid();
            string fileName = Path.GetFileName(uploadedFile.FileName);
            string filetype = Path.GetExtension(fileName);
            string s3Key = $"{guid1}";

            // S3'e dosya yükleme işlemi
            bool uploadSuccess = _s3Service.UploadFileToS3(uploadedFile, s3Key);


            // Yükleme işlemi başarısızsa hata
            if (!uploadSuccess)
                throw new Exception("Dosya Amazon S3'e yüklenemedi.");

            // Dosya bilgilerini veritabanına kaydetmek için FileRecord nesnesi oluşturma
            var fileRecord = new FileRecord
            {
                Id = guid,
                FileName = fileName,
                S3Key = s3Key,
                UploadDate = DateTime.Now,
                IsDeleted = false,
                FileType = filetype
            };
            // Veritabanına dosya kaydı ekleme
            _fileRepo.AddFile(fileRecord);
        }
        public MemoryStream DownloadFile(string s3key)// Dosya indirme işlemi
        {
            // S3'ten dosyayı indirmek için S3Service sınıfının DownloadFileFromS3 metodunu kullanıyoruz
            var stream = _s3Service.DownloadFileFromS3(s3key);

            return stream;
        }


        public IEnumerable<FileDto> GetAllActiveFiles()// Aktif dosyaların alınması
        {
            // Veritabanından silinmemiş dosyaları alıyoruz ve upload tarihine göre azalan sırada sıralıyoruz
            var records = _fileRepo.GetAllFiles()
                                   .Where(f => !f.IsDeleted)
                                   .OrderByDescending(f => f.UploadDate);

            return records.Select(f => new FileDto
            {
                Id = f.Id,
                FileName = f.FileName,
                UploadDate = f.UploadDate
            });
        }

        public FileDto GetFileById(Guid id)// Dosya ID'sine göre dosya bilgilerini almayı sağayan metod
        {
            // Veritabanından dosya kaydını alıyoruz
            var record = _fileRepo.GetFileById(id);
            if (record == null || record.IsDeleted)
                return null;

            // Alınan dosya kaydını FileDto nesnesine dönüştürüyoruz
            return new FileDto
            {
                Id = record.Id,
                FileName = record.FileName,
                S3Key = record.S3Key,
                UploadDate = record.UploadDate
            };
        }

        public void SoftDeleteFile(Guid id)// Dosya silme methodu
        {
            _fileRepo.SoftDeleteFile(id);
        }
    }
}
