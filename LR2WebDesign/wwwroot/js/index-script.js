document.addEventListener('DOMContentLoaded', async () => {

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


    var buttonsCheaps = document.querySelectorAll('.popular-courses__cheaps > button');

    var responseCourses = await fetch("https://localhost:7280/courses.json");
    var coursesJson = await responseCourses.json();

    buttonsCheaps.forEach((btn, index) => {
        btn.addEventListener('click', async () => {
            var key = btn.textContent;
            await loadAndRenderCourses(key);
        });
    });


    async function loadAndRenderCourses(key) {

        var courses = coursesJson[key];
        var container = document.querySelector('.course-cards');
        container.innerHTML = '';
        if (!courses) {
            console.warn(`Категория "${key}" не найдена`);
            return;
        }
        courses.forEach((course, index) => {
            var courseCard = document.createElement('a');

            courseCard.classList.add('course-card');
            courseCard.href = course.url;
            courseCard.target = '_blank';
            courseCard.rel = 'noopener noreferrer';

            if (index === 0) {
                courseCard.dataset.testid = 'first_course_card_test';
            }

            courseCard.innerHTML = `
                                        <img src="images/course_logos/${course.image}" alt="${course.title}" width="128" height="136">
                                        <div class="course-card__text">
                                            <span>${course.title}</span>
                                            <span>${course.description}</span>
                                        </div>
                                    `;

            container.appendChild(courseCard);
        });


    }

});