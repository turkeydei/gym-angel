// ====== Helpers ======
const API_BASE =
    (window.__ENV__ && window.__ENV__.API_BASE) || "";  // fallback

if (!API_BASE) {
    console.error("env.js not loaded or API_BASE missing");
    alert("Config error: env.js not loaded. Please check <script src=\"/js/env.js\"> and its order.");
}


const API = (path) => `${window.__ENV__.API_BASE}${path}`;

function saveToken(token) {
    localStorage.setItem('ga_token', token);
}
function getToken() {
    return localStorage.getItem('ga_token');
}
function logout() {
    localStorage.removeItem('ga_token');
    window.location.href = 'signin.html';
}
// Base URL fallback nếu env.js chưa load

// Gọi API có/không kèm token
async function apiFetch(path, options = {}) {
    const headers = { 'Content-Type': 'application/json', ...(options.headers || {}) };
    const token = getToken();
    if (token) headers['Authorization'] = `Bearer ${token}`;
    const res = await fetch(API(path), { ...options, headers });
    let data = null;
    try { data = await res.json(); } catch { /* no body */ }
    if (!res.ok) {
        const msg = data?.message || data?.error || `HTTP ${res.status}`;
        throw new Error(msg);
    }
    return data;
}

// ====== Sign Up ======
async function handleSignup(e) {
    e.preventDefault();
    const userName = document.getElementById('username').value.trim();
    const fullName = document.getElementById('name').value.trim();
    const email    = document.getElementById('email').value.trim();
    const password = document.getElementById('password').value.trim();
    const confirm  = document.getElementById('confirmPassword').value.trim();

    if (!userName || !fullName || !email || !password) return alert('Please fill all required fields.');
    if (password !== confirm) return alert('Passwords do not match');

    const body = { UserName: userName, Email: email, FullName: fullName, Password: password };

    try {
        const data = await apiFetch('/api/auth/register', { method: 'POST', body: JSON.stringify(body) });
        // API của bạn trả { message: "..." }
        alert(data.message || 'Registered successfully');
        window.location.href = 'signin.html';
    } catch (err) {
        alert(`Sign up failed: ${err.message}`);
    }
}

// ====== Sign In ======
async function handleSignin(e) {
    e.preventDefault();
    const emailOrUser = document.getElementById('email').value.trim();     // bạn đang dùng field "email" trong UI
    const password    = document.getElementById('password').value.trim();

    if (!emailOrUser || !password) return alert('Please enter your credentials.');

    // LoginDTO của bạn: UsernameOrEmail + Password
    const body = { UsernameOrEmail: emailOrUser, Password: password };

    try {
        const data = await apiFetch('/api/auth/login', { method: 'POST', body: JSON.stringify(body) });
        // API trả { token: "..." }
        if (!data?.token) throw new Error('No token returned');
        saveToken(data.token);
        alert('Login success');
        // Ví dụ: quay về trang chủ
        window.location.href = 'index.html';
    } catch (err) {
        alert(`Login failed: ${err.message}`);
    }
}

// ====== Gắn vào form nếu có ======
document.addEventListener('DOMContentLoaded', () => {
    const signupForm = document.getElementById('signupForm');
    const signinForm = document.getElementById('loginForm');
    if (signupForm) signupForm.addEventListener('submit', handleSignup);
    if (signinForm) signinForm.addEventListener('submit', handleSignin);

    // Gắn cho nút logout nếu trang có
    const logoutBtn = document.getElementById('btnLogout');
    if (logoutBtn) logoutBtn.addEventListener('click', logout);
});

// ====== Ví dụ gọi API bảo vệ bằng JWT (sau khi login) ======
async function loadProductsDemo() {
    try {
        const products = await apiFetch('/api/products', { method: 'GET' });
        console.log('products:', products);
    } catch (e) {
        console.error(e);
    }
}
window.loadProductsDemo = loadProductsDemo;
