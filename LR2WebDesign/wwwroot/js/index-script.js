document.addEventListener('DOMContentLoaded', () => {

    const ai_constructor_button = document.querySelector('.ai-constructor-btn');
    ai_constructor_button.addEventListener('click', () => {
        window.location.href = 'ai-constructor';
    });

    const signin_button = document.querySelector('.sign-in_btn');
    signin_button.addEventListener('click', () => {
        window.location.href = 'authorization';
    });

    const html = document.documentElement;
    const change_theme_button = document.querySelector('.theme-toggle');
    change_theme_button.addEventListener('click', () => {
        const next = html.dataset.theme === 'dark' ? 'light' : 'dark';
        html.dataset.theme = next;
    });

    const io = new IntersectionObserver(entries => {
        entries.forEach(e => {
            if (e.isIntersecting) {
                e.target.classList.add('is-visible');
                io.unobserve(e.target);
            }
        });
    }, { threshold: 0.15 });

    var revealElems = document.querySelectorAll('.reveal');
    revealElems.forEach(el => io.observe(el));
});