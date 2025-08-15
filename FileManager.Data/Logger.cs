using FileManager.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileManager.Data
{
    public class Logger : ILogger
    {
        // Loglama işlemleri için Logger sınıfı
        public void LogError(string name, string message, Exception ex)// Loglama işlemi için hata mesajı ve exception
        {

            using (var conn = ConnectionHelper.GetConnection())
            {
                // Hata mesajını veritabanına eklemek için SQL sorgusu
                string query = "INSERT INTO logs(log_date, file_name, state,  message, exception) " +
                               "VALUES(@log_date,@file_name, @state,  @message, @exception)";

                // Bağlantı ve komut nesnelerini kullanarak veritabanına ekleme işlemi
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("log_date", DateTime.Now);
                    cmd.Parameters.AddWithValue("file_name", name);
                    cmd.Parameters.AddWithValue("state", "Error");
                    cmd.Parameters.AddWithValue("message", message);
                    cmd.Parameters.AddWithValue("exception", ex.ToString());

                    // Bağlantıyı açıyoruz ve sorguyu çalıştırıyoruz
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void LogInfo(string name, string message)// Loglama işlemi için bilgi mesajı
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                // Bilgi mesajını veritabanına eklemek için SQL sorgusu
                string query = "INSERT INTO logs(log_date, file_name, state,  message, exception) " +
                               "VALUES(@log_date,@file_name, @state,  @message, @exception)";

                // Bağlantı ve komut nesnelerini kullanarak veritabanına ekleme işlemi
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("log_date", DateTime.Now);
                    cmd.Parameters.AddWithValue("file_name", name);
                    cmd.Parameters.AddWithValue("state", "Info");
                    cmd.Parameters.AddWithValue("message", message);
                    cmd.Parameters.AddWithValue("exception", " ");

                    // Bağlantıyı açıyoruz ve sorguyu çalıştırıyoruz
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public IEnumerable<LogRecord> GetAllLogs()
        {
            var logs = new List<LogRecord>();

            using (var conn = ConnectionHelper.GetConnection())
            {

                string query = "SELECT * FROM logs";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    conn.Open();

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        logs.Add(new LogRecord
                        {
                            LogDate = reader.GetDateTime(reader.GetOrdinal("log_date")),
                            FileName = reader.GetString(reader.GetOrdinal("file_name")),
                            State = reader.GetString(reader.GetOrdinal("state")),
                            Message = reader.GetString(reader.GetOrdinal("message")),
                            Exception = reader.IsDBNull(reader.GetOrdinal("exception")) ? null : reader.GetString(reader.GetOrdinal("exception"))
                        });
                    }
                }
            }

            return logs;
        }
    }
}
