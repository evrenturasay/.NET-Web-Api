using Npgsql;
using System;
using System.Configuration;

namespace FileManager.Data
{
    // Bağlantı yönetimi için yardımcı class
    public static class ConnectionHelper
    {
        // Bu metot, app.config veya web.config dosyasındaki "DefaultConnection" isimli bağlantı dizesini kullanarak NpgsqlConnection nesnesi oluşturur.
        public static NpgsqlConnection GetConnection()
        {
            string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


            return new NpgsqlConnection(connString);
        }
    }
}
