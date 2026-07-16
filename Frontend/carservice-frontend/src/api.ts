import axios from 'axios';

const API_BASE_URL = 'https://carservice-api-hu-e8bnhmf9h9g2gwg8.polandcentral-01.azurewebsites.net/api';

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

export default api;