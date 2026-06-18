<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useNotesStore } from '@/stores/note'
import type { CreateNoteRequest, UpdateNoteRequest } from '@/api/noteApi'
import type { AxiosError } from 'axios'
import type { ErrorResponse } from '@/types/api'

const route = useRoute()
const router = useRouter()
const notesStore = useNotesStore()

const isEdit = !!route.params.id
const saving = ref(false)
const error = reactive({ message: '' })

const form = reactive<CreateNoteRequest & UpdateNoteRequest>({
  title: '',
  content: '',
})

onMounted(async () => {
  if (isEdit) {
    const id = route.params.id as string
    const note = await notesStore.fetchNoteById(id)
    form.title = note.title
    form.content = note.content ?? ''
  }
})

async function handleSubmit() {
  error.message = ''
  saving.value = true
  try {
    if (isEdit) {
      await notesStore.updateNote(route.params.id as string, form)
    } else {
      await notesStore.createNote(form as CreateNoteRequest)
    }
    router.push('/notes')
  } catch (e) {
    const axiosError = e as AxiosError<ErrorResponse>
    error.message = axiosError.response?.data?.message || 'Failed to save note'
  } finally {
    saving.value = false
  }
}
</script>

<template>
  <div class="mx-auto max-w-3xl px-4 py-8">
    <button
      @click="router.push('/notes')"
      class="mb-4 text-sm text-blue-600 hover:text-blue-500"
    >
      &larr; Back to Notes
    </button>

    <div class="rounded-lg border border-gray-200 bg-white p-6 shadow-sm">
      <h1 class="mb-6 text-2xl font-bold text-gray-900">
        {{ isEdit ? 'Edit Note' : 'New Note' }}
      </h1>
      <form @submit.prevent="handleSubmit" class="space-y-4">
        <div>
          <label for="title" class="block text-sm font-medium text-gray-700">Title *</label>
          <input
            id="title"
            v-model="form.title"
            type="text"
            required
            class="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
          />
        </div>
        <div>
          <label for="content" class="block text-sm font-medium text-gray-700">Content</label>
          <textarea
            id="content"
            v-model="form.content"
            rows="12"
            class="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
          ></textarea>
        </div>
        <p v-if="error.message" class="text-sm text-red-600">{{ error.message }}</p>
        <div class="flex gap-3">
          <button
            type="submit"
            :disabled="saving"
            class="rounded-md bg-blue-600 px-4 py-2 text-white hover:bg-blue-700 disabled:opacity-50"
          >
            {{ saving ? 'Saving...' : 'Save' }}
          </button>
          <button
            type="button"
            @click="router.push('/notes')"
            class="rounded-md bg-gray-100 px-4 py-2 text-gray-700 hover:bg-gray-200"
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  </div>
</template>
