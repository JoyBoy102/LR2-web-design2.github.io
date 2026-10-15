document.addEventListener('DOMContentLoaded', () => {

    const reg_button = document.querySelector('.submit-btn');

    reg_button.addEventListener('click', async (event) => {
        event.preventDefault();

        var user_login = document.querySelector('.login-input').value;
        var user_password = document.querySelector('.password-input').value;
        var user_email = document.querySelector('.email-input').value;

        const userPayload = {
            login: user_login,
            email: user_email,
            password: user_password
        };

        try {
            const response = await fetch('https://localhost:7280/registration/create', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(userPayload)
            });

            if (!response.ok) {
                throw new Error(`Ошибка сервера: ${response.status}`);
            }

            const data = await response.json();
            console.log('Успешно отправлено! Ответ сервера:', data);
        } catch (error) {
            console.error('Не удалось выполнить запрос:', error);
        }
    });
});
