import axios from 'axios';

const api = axios.create({
    baseURL: 'https://localhost:7282/api', // Updated to match launchSettings.json
    headers: {
        'Content-Type': 'application/json',
    },
});

// Interceptor to add token to every request
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Interceptor to handle 401 errors
api.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response && error.response.status === 401) {
            // Optional: Trigger logout or redirect
            // For now we just reject, but AuthContext could listen to this
            localStorage.removeItem('token');
            window.location.href = '/login';
        }
        return Promise.reject(error);
    }
);

export default api;
