
document.addEventListener("DOMContentLoaded", function () {

    //SweetAlert ile hata ve bilgi mesajlarını gösterme
    if (typeof errorMessage !== 'undefined' && errorMessage) {
        // Hata mesajı varsa SweetAlert ile göster
        Swal.fire({
            title: 'Error!',
            text: errorMessage,
            icon: 'error',
            confirmButtonText: 'OK'
        });
    }
    else if (typeof infoMessage !== 'undefined' && infoMessage) {
        // Bilgi mesajı varsa SweetAlert ile göster
        Swal.fire({
            title: 'Success!',
            text: infoMessage,
            icon: 'success',
            confirmButtonText: 'OK'
        });
    }

    var sidebar = document.getElementById("sidebar");// Sidebar
    var toggleBtn = document.getElementById("toggle-btn");//memü açma butonu
    var closeBtn = document.getElementById("close-btn");// Sidebar'ı kapatma butonu
    var content = document.getElementById("content");// İçerik alanı

    // Sidebar'ı açma 
    toggleBtn.addEventListener("click", function () {
        sidebar.classList.add("open");
        content.classList.add("shift");
        toggleBtn.style.display = "none";
    });
    sss
    // Sidebar'ı kapatma
    closeBtn.addEventListener("click", function () {
        sidebar.classList.remove("open");
        content.classList.remove("shift");
        toggleBtn.style.display = "block";
    });
});