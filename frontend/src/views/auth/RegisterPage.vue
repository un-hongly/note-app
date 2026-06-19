<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import type { AxiosError } from 'axios'
import type { RegisterRequest } from '@/api/authApi'
import { useAuthStore } from '@/stores/auth'
import type { ErrorResponse } from '@/types/api'

const router = useRouter()
const auth = useAuthStore()

const form = reactive<RegisterRequest>({
  firstName: '',
  lastName: '',
  username: '',
  password: '',
  passwordConfirm: '',
})

const error = reactive({ message: '' })
const loading = ref(false)

async function handleSubmit() {
  error.message = ''
  loading.value = true
  try {
    await auth.register(form)
    router.push('/login')
  } catch (e) {
    const axiosError = e as AxiosError<ErrorResponse>
    error.message = axiosError.response?.data?.message || 'Registration failed'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="flex min-h-screen items-center justify-center bg-gray-50 px-4">
    <div class="w-full max-w-md rounded-lg bg-white p-8 shadow-md">
      <h1 class="mb-6 text-2xl font-bold text-gray-900">Create Account</h1>
      <form @submit.prevent="handleSubmit" class="space-y-4">
        <div class="flex gap-4">
          <div class="flex-1">
            <label for="firstName" class="block text-sm font-medium text-gray-700">First Name</label>
            <input
              id="firstName"
              v-model="form.firstName"
              type="text"
              required
              class="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
            />
          </div>
          <div class="flex-1">
            <label for="lastName" class="block text-sm font-medium text-gray-700">Last Name</label>
            <input
              id="lastName"
              v-model="form.lastName"
              type="text"
              required
              class="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
            />
          </div>
        </div>
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
        <div>
          <label for="passwordConfirm" class="block text-sm font-medium text-gray-700">Confirm Password</label>
          <input
            id="passwordConfirm"
            v-model="form.passwordConfirm"
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
          <span v-if="!loading">Register</span>
          <span v-else>Registering...</span>
        </button>
      </form>
      <p class="mt-4 text-center text-sm text-gray-600">
        Already have an account?
        <router-link to="/login" class="text-blue-600 hover:text-blue-500">Sign In</router-link>
      </p>
    </div>
  </div>
</template>
