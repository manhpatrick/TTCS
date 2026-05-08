document.addEventListener('DOMContentLoaded', function () {
    loadFooter();
});

async function loadFooter() {
    try {
        const response = await fetch('/footer.html');
        if (!response.ok) throw new Error('Network response was not ok');
        const html = await response.text();

        const footerPlaceholder = document.getElementById('footer-placeholder');
        if (footerPlaceholder) {
            footerPlaceholder.innerHTML = html;
        }
    } catch (error) {
        console.error('Lỗi khi tải footer:', error);
    }
}