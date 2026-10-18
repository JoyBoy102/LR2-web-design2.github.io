document.addEventListener('DOMContentLoaded', () => {

    const login_button = document.querySelector('.submit-btn');

    login_button.addEventListener('click', async (event) => {
        event.preventDefault();

        var tokenKey = "accessToken";
        var user_login_or_email = document.querySelector('.login-or-email-input').value;
        var user_password = document.querySelector('.password-input').value;

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;


        const userPayload = {
            loginoremail: user_login_or_email,
            password: user_password,
        };
        const toast = Swal.mixin({
            showConfirmButton: false,
            showCancelButton: false,
            position: 'top-end',
            timer: 3000,
            toast: true,
            timerProgressBar: true,
            customClass: {
                popup: 'colored-toast',
            },
        });

        try {
            const response = await fetch('https://localhost:7280/authorization/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(userPayload)
            });

            if (!response.ok) {
                const data = await response.json();
                const msg = data.errors ? Object.values(data.errors).flat().join(', ') : 'Ошибка данных';
                toast.fire({
                    icon: 'warning',
                    title: 'Warning',
                    text: msg
                })
                throw new Error(`Ошибка сервера: ${response.status}`);
            }

            const data = await response.json();
            localStorage.setItem(tokenKey, data.accessToken);
            window.location.href = data["redirectUrl"];
            console.log('Успешно отправлено! Ответ сервера:', data);
        } catch (error) {
            console.error('Не удалось выполнить запрос:', error);
        }
    });
});
