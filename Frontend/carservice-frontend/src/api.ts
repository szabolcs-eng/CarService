import axios from 'axios';

// Configurable per environment via Vite env vars - see .env.example.
// Falls back to the deployed API so `npm run dev` still works out of the box,
// but a real local/staging setup should set VITE_API_URL explicitly.
const API_BASE_URL =
  import.meta.env.VITE_API_URL ??
  'https://carservice-api-hu-e8bnhmf9h9g2gwg8.polandcentral-01.azurewebsites.net/api';

const api = axios.create({
  baseURL: API_BASE_URL,
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// If the token is missing/expired/invalid, the API returns 401 - bounce to login
// instead of leaving the app in a broken, half-authenticated state.
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      if (window.location.pathname !== '/login') {
        window.location.href = '/login';
      }
    }
    return Promise.reject(error);
  },
);

export default api;
