document.addEventListener('DOMContentLoaded', () => {

    const reg_button = document.querySelector('.submit-btn');

    reg_button.addEventListener('click', async (event) => {
        event.preventDefault();

        var user_login = document.querySelector('.login-input').value;
        var user_password = document.querySelector('.password-input').value;
        var user_email = document.querySelector('.email-input').value;
        var user_name = crypto.randomUUID();
        var tokenKey = "accessToken";

        const userPayload = {
            login: user_login,
            email: user_email,
            password: user_password,
            name: user_name
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
            const response = await fetch('https://localhost:7280/registration/create', {
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
