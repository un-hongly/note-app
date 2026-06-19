import axios from 'axios'
import type { AxiosInstance, AxiosError } from 'axios'
import type { ErrorResponse } from '@/types/api'

const http: AxiosInstance = axios.create({
   baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

http.interceptors.request.use((config) => {
  const token = localStorage.getItem('accessToken')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

http.interceptors.response.use(
  (response) => response,
  (error: AxiosError<ErrorResponse>) => {
    if (error.response?.status === 401) {
      // const hadToken = !!localStorage.getItem('accessToken')
      localStorage.removeItem('accessToken')
      // if (hadToken) {
      //   window.location.replace('/login')
      // }
    }
    return Promise.reject(error)
  },
)

export default http
