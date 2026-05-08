let globalNotificationsList = []; // Biến toàn cục lưu danh sách thông báo

document.addEventListener('DOMContentLoaded', function () {
    loadHeader();
});

async function loadHeader() {
    try {
        const response = await fetch('/header.html');
        const html = await response.text();
        document.getElementById('header-placeholder').innerHTML = html;

        checkAuthStatus();
        if (typeof setActiveNavLink === 'function') {
            setActiveNavLink();
        }
    } catch (error) {
        console.error('Lỗi khi tải header:', error);
    }
}

function getHeaderAuthToken() {
    return localStorage.getItem('userToken') || localStorage.getItem('demo_auth_token');
}

function checkAuthStatus() {
    const authContainer = document.getElementById('authContainer');
    const user = localStorage.getItem('demo_auth_user');
    const token = getHeaderAuthToken();

    if (user && token) {
        authContainer.innerHTML = `
            <div class="flex items-center gap-4 relative">
                <div class="relative">
                    <button id="notiBellBtn" class="p-2 hover:bg-surface-container rounded-full transition-colors flex items-center justify-center relative">
                        <span class="material-symbols-outlined text-primary">notifications</span>
                        <span id="notiBadge" class="absolute top-1 right-1 w-4 h-4 bg-error text-[10px] text-white rounded-full flex items-center justify-center" style="display: none;">0</span>
                    </button>
                    <div id="notiDropdown" class="absolute top-full right-0 mt-3 w-[350px] bg-surface rounded-xl editorial-shadow border border-outline-variant/20 opacity-0 invisible translate-y-2 transition-all duration-300 z-50">
                        <div class="p-4 border-b border-outline-variant/10 flex justify-between items-center bg-surface-container-lowest rounded-t-xl">
                            <h3 class="font-bold text-primary">Thông báo</h3>
                        </div>
                        <div class="max-h-[400px] overflow-y-auto p-2 custom-scrollbar" id="notiList">
                            <div class="p-8 text-center text-sm text-on-surface-variant">
                                <span class="material-symbols-outlined text-3xl mb-2 opacity-50">hourglass_empty</span>
                                <p>Đang tải thông báo...</p>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="relative">
                    <button id="userMenuBtn" class="flex items-center gap-2 group">
                        <div class="w-10 h-10 rounded-full overflow-hidden border-2 border-primary/10 group-hover:border-secondary transition-all bg-surface-container">
                            <img src="https://ui-avatars.com/api/?name=${user}&background=003345&color=fff" class="w-full h-full object-cover"/>
                        </div>
                        <span class="text-sm font-semibold text-primary hidden sm:inline">${user}</span>
                        <span class="material-symbols-outlined text-primary/60">expand_more</span>
                    </button>
                    <div id="userDropdown" class="absolute top-full right-0 mt-3 w-56 bg-surface rounded-xl editorial-shadow border border-outline-variant/20 opacity-0 invisible translate-y-2 transition-all duration-300 z-50 overflow-hidden">
                        <div class="p-2">
                            <a href="/user_profile.html" class="flex items-center gap-3 px-3 py-2 text-sm hover:bg-surface-container rounded-lg transition-colors text-on-surface">
                                <span class="material-symbols-outlined text-[20px]">person</span> Hồ sơ
                            </a>
                            <a href="/booking.html" class="flex items-center gap-3 px-3 py-2 text-sm hover:bg-surface-container rounded-lg transition-colors text-on-surface">
                                <span class="material-symbols-outlined text-[20px]">book_online</span> Đặt chỗ của tôi
                            </a>
                            <a href="javascript:void(0)" onclick="openFeedbackModal()" class="flex items-center gap-3 px-3 py-2 text-sm hover:bg-surface-container rounded-lg transition-colors text-on-surface">
                                <span class="material-symbols-outlined text-[20px]">rate_review</span> Phản hồi góp ý
                            </a>
                            <hr class="my-2 border-outline-variant/10">
                            <button onclick="logout()" class="w-full flex items-center gap-3 px-3 py-2 text-sm text-error hover:bg-error-container/30 rounded-lg transition-colors text-left">
                                <span class="material-symbols-outlined text-[20px]">logout</span> Đăng xuất
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        `;

        setupDropdowns();
        injectModalsHTML(); // Chèn các Modal (Noti, Feedback) ẩn vào trang
        fetchNotifications();
    } else {
        authContainer.innerHTML = `
            <a href="/user_auth.html" class="bg-primary text-white px-6 py-2.5 rounded-full text-sm font-bold transition-all hover:bg-primary-container active:scale-95 shadow-md">
                Đăng nhập
            </a>
        `;
    }
}

function setActiveNavLink() {
    const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('.nav-link');
    navLinks.forEach(link => {
        const href = link.getAttribute('href');
        link.classList.remove('text-secondary', 'border-b-2', 'border-secondary', 'pb-1');
        link.classList.add('text-primary/70', 'hover:text-secondary');
        if ((href === '/' && currentPath === '/') || (href !== '/' && currentPath.includes(href))) {
            link.classList.remove('text-primary/70', 'hover:text-secondary');
            link.classList.add('text-secondary', 'border-b-2', 'border-secondary', 'pb-1');
        }
    });
}

function setupDropdowns() {
    const userBtn = document.getElementById('userMenuBtn');
    const userDrop = document.getElementById('userDropdown');
    const notiBtn = document.getElementById('notiBellBtn');
    const notiDrop = document.getElementById('notiDropdown');

    userBtn?.addEventListener('click', (e) => {
        e.stopPropagation();
        notiDrop?.classList.add('opacity-0', 'invisible', 'translate-y-2');
        userDrop?.classList.toggle('opacity-0');
        userDrop?.classList.toggle('invisible');
        userDrop?.classList.toggle('translate-y-2');
    });

    notiBtn?.addEventListener('click', (e) => {
        e.stopPropagation();
        userDrop?.classList.add('opacity-0', 'invisible', 'translate-y-2');
        notiDrop?.classList.toggle('opacity-0');
        notiDrop?.classList.toggle('invisible');
        notiDrop?.classList.toggle('translate-y-2');

        if (!notiDrop.classList.contains('invisible')) {
            fetchNotifications();
        }
    });

    document.addEventListener('click', () => {
        userDrop?.classList.add('opacity-0', 'invisible', 'translate-y-2');
        notiDrop?.classList.add('opacity-0', 'invisible', 'translate-y-2');
    });
}

// ==========================================
// TỰ ĐỘNG CHÈN MÃ HTML CHO CÁC MODAL (THÔNG BÁO VÀ GÓP Ý)
// ==========================================
function injectModalsHTML() {
    // 1. Chèn Modal Thông Báo Chi Tiết
    if (!document.getElementById('notiDetailModal')) {
        const notiModalHTML = `
            <div id="notiDetailModalBackdrop" class="fixed inset-0 bg-black/60 z-[100] hidden transition-opacity duration-300" onclick="closeNotiModal()"></div>
            <div id="notiDetailModal" class="fixed top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 bg-surface w-[90%] max-w-md rounded-2xl p-8 z-[110] hidden shadow-2xl transition-all duration-300">
                <div class="flex justify-between items-start mb-2">
                    <h3 id="notiDetailTitle" class="text-2xl font-headline text-primary font-bold pr-4">Tiêu đề</h3>
                    <button onclick="closeNotiModal()" class="text-on-surface-variant hover:text-error transition-colors shrink-0">
                        <span class="material-symbols-outlined">close</span>
                    </button>
                </div>
                <p id="notiDetailDate" class="text-[11px] text-outline tracking-widest font-semibold uppercase mb-6">Ngày</p>
                <div class="bg-surface-container-low p-5 rounded-xl border border-outline-variant/10">
                    <p id="notiDetailContent" class="text-sm text-on-surface leading-relaxed whitespace-pre-wrap font-body"></p>
                </div>
                <div class="mt-8 flex justify-end">
                    <button onclick="closeNotiModal()" class="px-6 py-2.5 bg-primary text-white rounded-lg text-sm font-bold hover:bg-primary-container transition-all">
                        Đóng
                    </button>
                </div>
            </div>
        `;
        document.body.insertAdjacentHTML('beforeend', notiModalHTML);
    }

    // 2. Chèn Modal Phản Hồi / Góp Ý
    if (!document.getElementById('feedbackModal')) {
        const feedbackModalHTML = `
            <div id="feedbackModalBackdrop" class="fixed inset-0 bg-black/60 z-[100] hidden transition-opacity duration-300" onclick="closeFeedbackModal()"></div>
            <div id="feedbackModal" class="fixed top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 bg-surface w-[90%] max-w-md rounded-2xl p-8 z-[110] hidden shadow-2xl transition-all duration-300">
                <div class="flex justify-between items-start mb-6">
                    <h3 class="text-2xl font-headline text-primary font-bold">Phản hồi góp ý</h3>
                    <button onclick="closeFeedbackModal()" class="text-on-surface-variant hover:text-error transition-colors shrink-0">
                        <span class="material-symbols-outlined">close</span>
                    </button>
                </div>
                
                <div id="feedbackMessages" class="mb-4 hidden text-sm p-3 rounded-lg"></div>

                <form id="feedbackForm" onsubmit="submitFeedback(event)">
                    <div class="space-y-4 font-body">
                        <div>
                            <label class="block text-[10px] uppercase tracking-widest text-on-surface-variant font-bold mb-2">Tiêu đề</label>
                            <input id="feedbackTitle" required type="text" class="w-full bg-surface-container-low border border-transparent focus:border-primary focus:ring-0 rounded-lg p-3 text-sm text-on-surface transition-colors outline-none" placeholder="Ví dụ: Đóng góp ý kiến về dịch vụ..." />
                        </div>
                        <div>
                            <label class="block text-[10px] uppercase tracking-widest text-on-surface-variant font-bold mb-2">Nội dung</label>
                            <textarea id="feedbackContent" required rows="5" class="w-full bg-surface-container-low border border-transparent focus:border-primary focus:ring-0 rounded-lg p-3 text-sm text-on-surface transition-colors outline-none custom-scrollbar" placeholder="Chia sẻ cảm nhận của bạn để chúng tôi phục vụ tốt hơn..."></textarea>
                        </div>
                        <button type="submit" id="submitFeedbackBtn" class="w-full bg-primary text-white py-3 rounded-lg text-sm font-bold hover:bg-primary-container shadow-md transition-all mt-4">
                            Gửi Phản Hồi
                        </button>
                    </div>
                </form>
            </div>
        `;
        document.body.insertAdjacentHTML('beforeend', feedbackModalHTML);
    }
}

// ==========================================
// LOGIC FEEDBACK / PHẢN HỒI GÓP Ý
// ==========================================
function openFeedbackModal() {
    // Đóng User Menu thả xuống
    const userDrop = document.getElementById('userDropdown');
    if (userDrop) userDrop.classList.add('opacity-0', 'invisible', 'translate-y-2');

    // Reset Form & Thông báo
    document.getElementById('feedbackForm')?.reset();
    document.getElementById('feedbackMessages')?.classList.add('hidden');

    // Mở Modal
    document.getElementById('feedbackModalBackdrop').classList.remove('hidden');
    document.getElementById('feedbackModal').classList.remove('hidden');
}

function closeFeedbackModal() {
    document.getElementById('feedbackModalBackdrop').classList.add('hidden');
    document.getElementById('feedbackModal').classList.add('hidden');
}

async function submitFeedback(event) {
    event.preventDefault();

    const title = document.getElementById('feedbackTitle').value.trim();
    const content = document.getElementById('feedbackContent').value.trim();
    const msgBox = document.getElementById('feedbackMessages');
    const btn = document.getElementById('submitFeedbackBtn');
    const token = getHeaderAuthToken();

    if (!token) {
        msgBox.className = "mb-4 text-sm p-3 rounded-lg bg-error-container text-error";
        msgBox.innerText = "Vui lòng đăng nhập lại để gửi góp ý.";
        msgBox.classList.remove('hidden');
        return;
    }

    btn.innerText = "Đang gửi...";
    btn.disabled = true;

    try {
        const response = await fetch('/api/customer/feedback', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            // Body chuẩn với FeedbackRequest (Title, Content)
            body: JSON.stringify({ Title: title, Content: content })
        });

        if (!response.ok) {
            throw new Error("Có lỗi xảy ra, không thể gửi phản hồi lúc này.");
        }

        msgBox.className = "mb-4 text-sm p-3 rounded-lg bg-[#e6f9f3] text-[#044d65]";
        msgBox.innerText = "Cảm ơn bạn đã đóng góp ý kiến!";
        msgBox.classList.remove('hidden');

        // Đóng modal sau 1.5s
        setTimeout(() => {
            closeFeedbackModal();
        }, 1500);

    } catch (error) {
        msgBox.className = "mb-4 text-sm p-3 rounded-lg bg-error-container text-error";
        msgBox.innerText = error.message;
        msgBox.classList.remove('hidden');
    } finally {
        btn.innerText = "Gửi Phản Hồi";
        btn.disabled = false;
    }
}

// ==========================================
// LOGIC THÔNG BÁO (NOTIFICATION)
// ==========================================
async function fetchNotifications() {
    const token = getHeaderAuthToken();
    if (!token) return;

    try {
        const response = await fetch('/api/customer/notification/me', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            const data = await response.json();
            globalNotificationsList = Array.isArray(data) ? data : (data.data || []);
            renderNotifications(globalNotificationsList);
        }
    } catch (error) {
        console.error("Lỗi tải thông báo:", error);
    }
}

function renderNotifications(notifications) {
    const notiList = document.getElementById("notiList");
    const notiBadge = document.getElementById("notiBadge");

    if (!notiList) return;

    const unreadCount = notifications.filter(n => !(n.isRead || n.IsRead)).length;

    if (notiBadge) {
        if (unreadCount > 0) {
            notiBadge.innerText = unreadCount > 99 ? '99+' : unreadCount;
            notiBadge.style.display = 'flex';
        } else {
            notiBadge.style.display = 'none';
        }
    }

    if (!notifications || notifications.length === 0) {
        notiList.innerHTML = `
            <div class="p-8 text-center text-sm text-on-surface-variant">
                <span class="material-symbols-outlined text-3xl mb-2 opacity-50">notifications_off</span>
                <p>Bạn không có thông báo nào.</p>
            </div>`;
        return;
    }

    notiList.innerHTML = notifications.map(noti => {
        const isRead = noti.isRead || noti.IsRead;
        const id = noti.id || noti.Id;
        const title = noti.title || noti.Title || 'Thông báo hệ thống';
        const message = noti.message || noti.Message || noti.content || noti.Content || '';
        const createdAt = noti.createdAt || noti.CreatedAt;

        const bgClass = isRead ? 'bg-transparent hover:bg-surface-container' : 'bg-primary/5 hover:bg-primary/10';
        const dotHtml = isRead ? '' : `<div class="w-2 h-2 rounded-full bg-error mt-1.5 flex-shrink-0"></div>`;

        let dateStr = 'Gần đây';
        if (createdAt) {
            const d = new Date(createdAt);
            dateStr = d.toLocaleDateString('vi-VN') + ' ' + d.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });
        }

        return `
            <div onclick="openNotiModal(${id})" class="p-3 rounded-xl cursor-pointer mb-1 flex gap-3 transition-colors ${bgClass}">
                ${dotHtml}
                <div class="flex-1">
                    <p class="text-sm font-bold ${isRead ? 'text-on-surface-variant' : 'text-primary'}">${title}</p>
                    <p class="text-[13px] text-on-surface-variant mt-1 leading-snug line-clamp-2">${message}</p>
                    <p class="text-[10px] text-outline mt-2 tracking-widest font-semibold uppercase">${dateStr}</p>
                </div>
            </div>
        `;
    }).join('');
}

async function openNotiModal(id) {
    const noti = globalNotificationsList.find(n => (n.id || n.Id) === id);
    if (!noti) return;

    const notiDrop = document.getElementById('notiDropdown');
    if (notiDrop) notiDrop.classList.add('opacity-0', 'invisible', 'translate-y-2');

    document.getElementById('notiDetailTitle').innerText = noti.title || noti.Title || 'Thông báo hệ thống';
    document.getElementById('notiDetailContent').innerText = noti.message || noti.Message || noti.content || noti.Content || '';

    const createdAt = noti.createdAt || noti.CreatedAt;
    if (createdAt) {
        const d = new Date(createdAt);
        document.getElementById('notiDetailDate').innerText = d.toLocaleDateString('vi-VN') + ' ' + d.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });
    } else {
        document.getElementById('notiDetailDate').innerText = 'Gần đây';
    }

    document.getElementById('notiDetailModalBackdrop').classList.remove('hidden');
    document.getElementById('notiDetailModal').classList.remove('hidden');

    const isRead = noti.isRead || noti.IsRead;
    if (!isRead) {
        try {
            const token = getHeaderAuthToken();
            await fetch(`/api/customer/notification/${id}`, {
                method: 'PUT',
                headers: { 'Authorization': `Bearer ${token}` }
            });
            fetchNotifications();
        } catch (error) {
            console.error("Lỗi cập nhật trạng thái đã đọc:", error);
        }
    }
}

function closeNotiModal() {
    document.getElementById('notiDetailModalBackdrop').classList.add('hidden');
    document.getElementById('notiDetailModal').classList.add('hidden');
}

function logout() {
    localStorage.removeItem('demo_auth_user');
    localStorage.removeItem('userToken');
    localStorage.removeItem('demo_auth_token');
    window.location.href = '/';
}