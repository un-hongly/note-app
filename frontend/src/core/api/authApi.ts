import type { User } from '../models/User'
import http from './api'


export interface RegisterRequest {
  firstName: string
  lastName: string
  username: string
  password: string
  passwordConfirm: string
}

export interface LoginRequest {
  username: string
  password: string
}

export interface LoginResponse {
  accessToken: string
}

export const authApi = {
  register(data: RegisterRequest) {
    return http.post<User>('/auth/register', data)
  },

  login(data: LoginRequest) {
    return http.post<LoginResponse>('/auth/login', data)
  },
}
