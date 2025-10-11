class ModernBrutalistSignupForm {
    constructor() {
        this.form = document.getElementById('signupForm');
        this.nameInput = document.getElementById('name');
        this.emailInput = document.getElementById('email');
        this.passwordInput = document.getElementById('password');
        this.confirmInput = document.getElementById('confirmPassword');
        this.submitButton = this.form.querySelector('.login-btn');
        this.successMessage = document.getElementById('successMessage');
        this.socialButtons = document.querySelectorAll('.social-btn');
        this.init();
    }

    init() {
        this.bindEvents();
    }

    bindEvents() {
        this.form.addEventListener('submit', (e) => this.handleSubmit(e));
    }

    validate() {
        let valid = true;

        if (!this.nameInput.value.trim()) {
            this.showError('name', 'Full name is required');
            valid = false;
        }

        if (!this.emailInput.value.trim()) {
            this.showError('email', 'Email is required');
            valid = false;
        }

        if (this.passwordInput.value.length < 6) {
            this.showError('password', 'Password must be at least 6 characters');
            valid = false;
        }

        if (this.passwordInput.value !== this.confirmInput.value) {
            this.showError('confirm', 'Passwords do not match');
            valid = false;
        }

        return valid;
    }

    showError(field, message) {
        const errorEl = document.getElementById(`${field}Error`);
        errorEl.textContent = message;
        errorEl.classList.add('show');
    }

    clearErrors() {
        document.querySelectorAll('.error-message').forEach(el => {
            el.textContent = '';
            el.classList.remove('show');
        });
    }

    async handleSubmit(e) {
        e.preventDefault();
        this.clearErrors();

        if (!this.validate()) return;

        this.setLoading(true);
        try {
            const res = await fetch(window.__ENV__.API_BASE + "/api/auth/register", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    name: this.nameInput.value.trim(),
                    email: this.emailInput.value.trim(),
                    password: this.passwordInput.value
                })
            });

            if (!res.ok) throw new Error("Registration failed");
            this.showSuccess();

            setTimeout(() => {
                window.location.href = "signin.html";
            }, 3000);
        } catch (err) {
            this.showError('email', err.message);
        } finally {
            this.setLoading(false);
        }
    }

    setLoading(loading) {
        this.submitButton.classList.toggle('loading', loading);
        this.submitButton.disabled = loading;
    }

    showSuccess() {
        this.form.style.display = 'none';
        this.successMessage.classList.add('show');
    }
}

document.addEventListener('DOMContentLoaded', () => new ModernBrutalistSignupForm());
document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('signupForm');
    const successBox = document.getElementById('successMessage');

    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        const userName = document.getElementById('username').value.trim();
        const fullName = document.getElementById('name').value.trim();
        const email    = document.getElementById('email').value.trim();
        const password = document.getElementById('password').value.trim();
        const confirm  = document.getElementById('confirmPassword').value.trim();

        if (password !== confirm) {
            document.getElementById('confirmError').textContent = 'Passwords do not match';
            return;
        }

        try {
            const res = await fetch(`${window.__ENV__.API_BASE}/api/auth/register`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ UserName: userName, Email: email, FullName: fullName, Password: password })
            });

            const data = await res.json();
            if (res.ok) {
                successBox.classList.add('show');
                setTimeout(() => (window.location.href = 'signin.html'), 2000);
            } else {
                // Controller có thể trả { message } hoặc { error }
                alert(data.message || data.error || 'Registration failed');
            }
        } catch (err) {
            alert(`Network error: ${err.message}`);
        }
    });
});

