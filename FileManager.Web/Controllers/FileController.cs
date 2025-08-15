using FileManager.Business;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileManager.Web.Controllers
{
    public class FileController : Controller
    {

        // Dosya ve log işlemleri için servisler
        private readonly IFileService _fileService;
        private readonly ILogService _logService;

        public FileController()
        {
            //Servislerin çağırılması
            _fileService = new FileService();
            _logService = new LogService();
        }

        public ActionResult Index() //Tüm dosyaların listelendiği sayfa
        {
            // Aktif dosyaların alınması ve ekranda listelenmesi
            var files = _fileService.GetAllActiveFiles();
            return View(files);
        }

        [HttpGet]
        public ActionResult Upload() //Dosya yükleme sayfası
        {
            return View();
        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase uploadedFile) //Dosya yükleme işlemi
        {
            if (uploadedFile != null && uploadedFile.ContentLength > 0) // Dosya kontrolü
            {
                try
                {

                    string[] allowedExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".xlsm" };

                    string extension = Path.GetExtension(uploadedFile.FileName).ToLower();

                    // Yüklenen dosyanın uzantısının kontrol edilmesi                    
                    if (!allowedExtensions.Contains(extension))
                    {

                        TempData["Error"] = "İnvalid File Type";
                        return RedirectToAction("Upload");
                    }

                    // Dosyanın S3'e yüklenmesi ve loglama işlemi
                    _fileService.UploadFile(uploadedFile);
                    _logService.LogInfo(uploadedFile.FileName, "File Uploaded.");// Loglama işlemi
                    TempData["Message"] = "File Uploaded.";
                }
                catch (Exception ex)
                {
                    // Hata durumunda loglama ve kullanıcıya mesaj gösterme
                    _logService.LogError(uploadedFile.FileName, "Upload Error ", ex);
                    TempData["Error"] = "An Error Occurred During Upload. ";
                    return RedirectToAction("Upload");
                }
            }
            else
            {
                // Dosya seçilmediğinde kullanıcıya uyarı verme ve yönlendirme
                TempData["Error"] = "Please Choose a Valid File.";
                return RedirectToAction("Upload");
            }

            // Yükleme işlemi başarılıysa ana sayfaya yönlendirme
            return RedirectToAction("Index");
        }


        public ActionResult Delete(Guid id) // Dosya silme sayfası
        {
            // Silinecek dosyanın ID'sine göre dosyanın alınması
            var file = _fileService.GetFileById(id);
            if (file == null)
                return HttpNotFound();

            return View(file);
        }

        // Silme işlemi için onay alındıktan sonra çalışacak metod
        [HttpPost, ActionName("DeleteConfirmed")]
        public ActionResult DeleteConfirmed(Guid id)// Dosya silme metodu
        {
            var file = _fileService.GetFileById(id);
            try
            {
                // Dosyanın soft delete işlemi
                _fileService.SoftDeleteFile(id);
                TempData["Message"] = "File Deleted.";
                _logService.LogInfo(file.FileName, "File Deleted");
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama ve kullanıcıya mesaj gösterme
                TempData["Error"] = "An Error Occured During Delete.";
                _logService.LogError(file.FileName, "Delete Error ", ex);
            }

            // Silme işlemi başarılıysa ana sayfaya yönlendirme
            return RedirectToAction("Index");
        }


        // İndirme işlemi için onay alındıktan sonra çalışacak metod
        [HttpPost, ActionName("DownloadConfirmed")]
        public ActionResult DownloadConfirmed(Guid id)
        {

            var file = _fileService.GetFileById(id);
            try
            {
                // Dosyanın Aws üzerinden indirilmesi ve loglama işlemi
                var stream = _fileService.DownloadFile(file.S3Key);// İndirilecek dosyanın S3Key'ine göre dosyanın indirilmesi
                _logService.LogInfo(file.FileName, "File Downloaded.");
                return File(stream, "application/octet-stream", file.FileName);// İndirilen dosyanın browser üzerinden çekilmesi
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama ve kullanıcıya mesaj gösterme
                TempData["Error"] = "An Error Occured During Download. ";
                _logService.LogError(file.FileName, "Download Error", ex);
                return RedirectToAction("Index");
            }

        }


    }
}


