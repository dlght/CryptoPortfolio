import axios from 'axios';

const baseURL = String(process.env.REACT_APP_API_BASE_URL);

const http = axios.create({
  baseURL,
  headers: {
  },
});

export const get = async <T>(url: string): Promise<T> => {
  const response = await http.get<T>(url);
  return response.data;
};

export const post = async <T, D>(url: string, data: D): Promise<T> => {
  const response = await http.post<T>(url, data);
  return response.data;
};

export default http;
