import http from 'k6/http';
import { check, sleep } from 'k6';
import { group } from 'k6';

export const options = {
    stages: [
        { duration: '1m', target: 50 },
        { duration: '30m', target: 50 },
        { duration: '1m', target: 0 },
    ],
    thresholds: {
        http_req_failed: ['rate<0.01'],
        http_req_duration: ['p(95)<800'],
    },
};

const BASE_URL = 'https://localhost:7280';

export default function () {
    group('1. Main Page', function () {
        const res = http.get(`${BASE_URL}/`);
        check(res, {
            'Main page is 200': (r) => r.status === 200,
        });
    });
    sleep(1);

    group('2. Login Page', function () {
        const res = http.get(`${BASE_URL}/authorization`);
        check(res, {
            'Login page is 200': (r) => r.status === 200,
        });
    });
    sleep(1);

    group('3. Login Action', function () {
        const payload = JSON.stringify({
            LoginOrEmail: 'user@example.com',
            Password: 'Password123@'
        });

        const params = {
            headers: {
                'Content-Type': 'application/json',
            },
        };

        const res = http.post(`${BASE_URL}/authorization/login`, payload, params);

        check(res, {
            'Login POST is 200': (r) => r.status === 200,
            'Has accessToken': (r) => r.json('accessToken') !== undefined,
        });
    });
    sleep(1);

    group('4. AI Constructor (Protected)', function () {
        const res = http.get(`${BASE_URL}/ai-constructor`);

        check(res, {
            'AI Constructor is 200': (r) => r.status === 200,
        });
    });
    sleep(1);
}