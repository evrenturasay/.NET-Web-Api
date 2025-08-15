document.addEventListener('DOMContentLoaded', function () {
    // Dosya yükleme alanı ve butonları
    const uploadArea = document.getElementById('uploadArea');// Drag and drop alanı
    const fileInput = document.getElementById('uploadedFile');// Dosya input alanı
    const selectedFile = document.getElementById('selectedFile');// Seçilen dosya bilgilerini gösteren alan
    const uploadContent = document.querySelector('.upload-content');// Yükleme içeriği alanı
    const uploadBtn = document.getElementById('uploadBtn');// Yükleme butonu
    const uploadProgress = document.getElementById('uploadProgress');// Yükleme ilerleme çubuğu alanı
    const progressBar = uploadProgress.querySelector('.progress-bar');// İlerleme çubuğu

    // Drag and drop olayları
    uploadArea.addEventListener('dragover', function (e) {
        e.preventDefault();
        uploadArea.classList.add('border-primary', 'bg-light');
    });

    uploadArea.addEventListener('dragleave', function (e) {
        e.preventDefault();
        uploadArea.classList.remove('border-primary', 'bg-light');
    });

    uploadArea.addEventListener('drop', function (e) {
        e.preventDefault();
        uploadArea.classList.remove('border-primary', 'bg-light');

        const files = e.dataTransfer.files;
        if (files.length > 0) {
            fileInput.files = files;
            handleFileSelection();
        }
    });

    // Dosya input alanına değişiklik olduğunda
    fileInput.addEventListener('change', handleFileSelection);

    function handleFileSelection() {
        // Seçilen dosyayı kontrol et
        const file = fileInput.files[0];
        if (file) {

            //Ddosya boyutunu kontrol et
            if (file.size > 20 * 1024 * 1024) {
                alert('Dosya boyutu 20MB\'dan büyük olamaz!');
                fileInput.value = '';
                return;
            }


            uploadContent.classList.add('d-none');// Yükleme içeriğini gizle
            selectedFile.classList.remove('d-none');// Seçilen dosya bilgisini göster
            selectedFile.querySelector('.file-name').textContent = file.name;// Seçilen dosya adını göster
            selectedFile.querySelector('.file-size').textContent = formatFileSize(file.size);// Seçilen dosya boyutunu göster
            uploadBtn.disabled = false;// Yükleme butonunu aktif et

            fileInput.classList.remove('is-invalid');
            uploadArea.classList.add('border-success');
        }
    }

    // Dosya boyutunun formatlanması
    function formatFileSize(bytes) {
        if (bytes === 0) return '0 Bytes';
        const k = 1024;
        const sizes = ['Bytes', 'KB', 'MB', 'GB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
    }

    // Yükleme butonuna tıklandığında
    document.querySelector('form').addEventListener('submit', function (e) {
        if (!fileInput.files[0]) {
            e.preventDefault();
            fileInput.classList.add('is-invalid');
            return;
        }

        // Yükleme işlemini başlat
        uploadProgress.classList.remove('d-none');
        uploadBtn.disabled = true;// Yükleme butonunu devre dışı bırak
        uploadBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Yükleniyor...';

        // İlerleme çubuğu
        let progress = 0;
        const interval = setInterval(() => {
            progress += Math.random() * 15;
            if (progress > 90) progress = 90;
            progressBar.style.width = progress + '%';

            if (progress >= 90) {
                clearInterval(interval);
            }
        }, 200);
    });
});