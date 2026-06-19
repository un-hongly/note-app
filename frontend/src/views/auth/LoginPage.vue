<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import type { LoginRequest } from '@/api/authApi'
import type { AxiosError } from 'axios'
import type { ErrorResponse } from '@/types/api'

const router = useRouter()
const auth = useAuthStore()

const form = reactive<LoginRequest>({
  username: '',
  password: '',
})

const error = reactive({ message: '' })
const loading = ref(false)

async function handleSubmit() {
  error.message = ''
  loading.value = true
  try {
    await auth.login(form)
    router.push('/notes')
  } catch (e) {
    const axiosError = e as AxiosError<ErrorResponse>
    error.message = axiosError.response?.data?.message || 'Login failed'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="flex min-h-screen items-center justify-center bg-gray-50 px-4">
    <div class="w-full max-w-md rounded-lg bg-white p-8 shadow-md">
      <h1 class="mb-6 text-2xl font-bold text-gray-900">Sign In</h1>
      <form @submit.prevent="handleSubmit" class="space-y-4">
        <div>
          <label for="username" class="block text-sm font-medium text-gray-700">Username</label>
          <input
            id="username"
            v-model="form.username"
            type="text"
            required
            class="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
          />
        </div>
        <div>
          <label for="password" class="block text-sm font-medium text-gray-700">Password</label>
          <input
            id="password"
            v-model="form.password"
            type="password"
            required
            class="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
          />
        </div>
        <p v-if="error.message" class="text-sm text-red-600">{{ error.message }}</p>
        <button
          type="submit"
          :disabled="loading"
          class="w-full rounded-md bg-blue-600 px-4 py-2 text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"
        >
          <svg v-if="loading" class="-ml-1 mr-2 inline h-4 w-4 animate-spin" fill="none" viewBox="0 0 24 24">
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" />
            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
          </svg>
          <span v-if="!loading">Sign In</span>
          <span v-else>Signing in...</span>
        </button>
      </form>
      <p class="mt-4 text-center text-sm text-gray-600">
        Don't have an account?
        <router-link to="/register" class="text-blue-600 hover:text-blue-500">Register</router-link>
      </p>
    </div>
  </div>
</template>
