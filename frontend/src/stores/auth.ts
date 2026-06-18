import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { authApi, type LoginRequest, type RegisterRequest } from '@/api/authApi'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('accessToken'))
  const isAuthenticated = computed(() => !!token.value)

  const login = async (data: LoginRequest) => {
    const res = await authApi.login(data)
    token.value = res.data.accessToken
    localStorage.setItem('accessToken', res.data.accessToken)
  }

  const register = async (data: RegisterRequest) => {
    await authApi.register(data)
  }

  const logout = () => {
    token.value = null
    localStorage.removeItem('accessToken')
  }

  return { token, isAuthenticated, login, register, logout }
})
