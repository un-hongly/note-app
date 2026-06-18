<script setup lang="ts">
import type { Note } from '@/core/models/Note'
import { useNotesStore } from '@/core/stores/note'
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
const notesStore = useNotesStore()

const note = ref<Note | null>(null)
const loading = ref(true)

onMounted(async () => {
  const id = route.params.id as string
  try {
    note.value = await notesStore.fetchNoteById(id)
  } finally {
    loading.value = false
  }
})

function formatDate(dateStr: string) {
  return new Date(dateStr).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
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

    <div v-if="loading" class="py-12 text-center text-gray-500">Loading...</div>

    <div v-else-if="!note" class="py-12 text-center text-gray-500">Note not found.</div>

    <div v-else class="rounded-lg border border-gray-200 bg-white p-6 shadow-sm">
      <h1 class="mb-2 text-2xl font-bold text-gray-900">{{ note.title }}</h1>
      <div class="mb-6 flex items-center gap-4 text-sm text-gray-500">
        <span>Created: {{ formatDate(note.createdAt) }}</span>
        <span>Updated: {{ formatDate(note.updatedAt) }}</span>
      </div>
      <div v-if="note.content" class="prose max-w-none whitespace-pre-wrap text-gray-700">
        {{ note.content }}
      </div>
      <p v-else class="text-gray-400 italic">No content</p>
      <div class="mt-6 flex gap-3">
        <router-link
          :to="`/notes/${note.id}/edit`"
          class="rounded-md bg-blue-600 px-4 py-2 text-white hover:bg-blue-700"
        >
          Edit
        </router-link>
        <button
          @click="notesStore.deleteNote(note.id); router.push('/notes')"
          class="rounded-md bg-red-600 px-4 py-2 text-white hover:bg-red-700"
        >
          Delete
        </button>
      </div>
    </div>
  </div>
</template>
