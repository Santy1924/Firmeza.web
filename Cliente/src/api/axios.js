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
            // Clear token but don't redirect - let the component handle it
            // This allows checkout errors to be displayed properly
            const currentPath = window.location.pathname;
            if (currentPath !== '/login' && currentPath !== '/register') {
                localStorage.removeItem('token');
                // Only redirect if we're not already handling an error in a component
                if (!error.config.url.includes('/checkout')) {
                    window.location.href = '/login';
                }
            }
        }
        return Promise.reject(error);
    }
);

export default api;
