document.addEventListener('DOMContentLoaded', () =>
{
    const app = document.querySelector('.container');
    const buttons = document.querySelectorAll('.toggle-sidebar-btn');

    buttons.forEach(btn => {
        btn.addEventListener('click', () => {
            app.classList.toggle('sidebar-collapsed');
        });
    });

});
