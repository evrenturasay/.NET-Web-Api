using System;
using System.Collections.Generic;
using FileManager.Models;
using Npgsql;


namespace FileManager.Data
{
    // Dosya işlemleri için repository sınıfı
    public class FileRepository : IFileRepository
    {

        public void AddFile(FileRecord file)// Dosya ekleme işlemi
        {
            // Veritabanına dosya kaydı eklemek için gerekli bağlantı ve komutları kullanıyoruz
            using (var conn = ConnectionHelper.GetConnection())
            {
                // Dosya bilgilerini veritabanına eklemek için SQL sorgusu
                string query = "INSERT INTO uploadedfiles (id, filename, s3key, uploaddate, isdeleted,filetype) " +
                               "VALUES (@id, @filename, @s3key, @uploaddate, @isdeleted,@filetype)";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    // Parametreleri ekliyoruz
                    cmd.Parameters.AddWithValue("id", file.Id);
                    cmd.Parameters.AddWithValue("filename", file.FileName);
                    cmd.Parameters.AddWithValue("s3key", file.S3Key);
                    cmd.Parameters.AddWithValue("uploaddate", file.UploadDate);
                    cmd.Parameters.AddWithValue("isdeleted", file.IsDeleted);
                    cmd.Parameters.AddWithValue("filetype", file.FileType);

                    // Bağlantıyı açıyoruz ve sorguyu çalıştırıyoruz
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<FileRecord> GetAllFiles()// Aktif dosyaların alınması
        {
            var files = new List<FileRecord>();

            using (var conn = ConnectionHelper.GetConnection())
            {
                // Tüm dosyaları almak için SQL sorgusu
                string query = "SELECT * FROM uploadedfiles";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    conn.Open();
                    // Sorguyu çalıştırıyoruz ve sonuçları okuyuyoruz
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // Her bir dosya kaydını FileRecord nesnesine dönüştürüyoruz
                        files.Add(new FileRecord
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            FileName = reader.GetString(reader.GetOrdinal("filename")),
                            S3Key = reader.GetString(reader.GetOrdinal("s3key")),
                            UploadDate = reader.GetDateTime(reader.GetOrdinal("uploaddate")),
                            IsDeleted = reader.GetBoolean(reader.GetOrdinal("isdeleted"))
                        });
                    }
                }
            }

            return files;
        }

        public FileRecord GetFileById(Guid id)// Dosya ID'sine göre dosya bilgilerini alma işlemi
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                // Belirli bir dosyayı ID'sine göre almak için SQL sorgusu
                string query = "SELECT * FROM uploadedfiles WHERE id = @id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    // Parametre olarak ID'yi ekliyoruz
                    cmd.Parameters.AddWithValue("id", id);
                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Dosya kaydını okuyup FileRecord nesnesine dönüştürüyoruz
                        return new FileRecord
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            FileName = reader.GetString(reader.GetOrdinal("filename")),
                            S3Key = reader.GetString(reader.GetOrdinal("s3key")),
                            UploadDate = reader.GetDateTime(reader.GetOrdinal("uploaddate")),
                            IsDeleted = reader.GetBoolean(reader.GetOrdinal("isdeleted"))
                        };
                    }
                }
            }

            return null;
        }

        public void SoftDeleteFile(Guid id)// Dosya silme işlemi
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                // Dosyayı soft delete yapmak için SQL sorgusu
                string query = "UPDATE uploadedfiles SET isdeleted = true WHERE id = @id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }



    }
}
