using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Web;
using System.Configuration;


namespace FileManager.AWS
{
    public class S3Service
    {
        // AWS S3 bağlantısı için gerekli değişkenler
        private readonly string bucketName;
        private readonly RegionEndpoint bucketRegion;
        private readonly IAmazonS3 s3Client;

        public S3Service()
        {
            // AWS S3 bağlantı bilgilerini web.config dosyasından alıyoruz
            var accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
            var secretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
            bucketName = ConfigurationManager.AppSettings["AWSBucketName"];
            var regionString = ConfigurationManager.AppSettings["AWSRegion"];
            bucketRegion = RegionEndpoint.GetBySystemName(regionString);
            s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
        }

        public bool UploadFileToS3(HttpPostedFileBase file, string s3Key)// Yükleme işlemi için gerekli metot
        {
            if (file == null || file.ContentLength == 0)
                return false;

            try
            {
                // S3'e yükleme işlemi için TransferUtility kullanıyoruz
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    // Dosya akışını ve gerekli bilgileri ayarlıyoruz
                    InputStream = file.InputStream,
                    Key = s3Key,
                    BucketName = bucketName,
                    ContentType = file.ContentType
                };
                // TransferUtility sınıfını kullanarak dosyayı S3'e yüklüyoruz
                var fileTransferUtility = new TransferUtility(s3Client);
                fileTransferUtility.Upload(uploadRequest);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public MemoryStream DownloadFileFromS3(string s3Key)// İndirme işlemi için gerekli metot
        {
            // S3'ten dosyayı indirmek için GetObjectRequest kullanıyoruz
            var request = new GetObjectRequest
            {
                // S3 bucket adı ve dosyanın S3 key'ini ayarlıyoruz
                BucketName = bucketName,
                Key = s3Key
            };
            // S3Client kullanarak dosyayı alıyoruz
            using (var response = s3Client.GetObject(request))
            using (var responseStream = response.ResponseStream)
            {
                // ResponseStream'den MemoryStream'e kopyalıyoruz böylece belleğe almış oluyor
                var memoryStream = new MemoryStream();
                responseStream.CopyTo(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
        }

    }
}


