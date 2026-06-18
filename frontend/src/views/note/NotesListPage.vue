<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useNotesStore } from '@/core/stores/note'
import { useAuthStore } from '@/core/stores/auth'

const router = useRouter()
const notesStore = useNotesStore()
const auth = useAuthStore()

onMounted(() => {
  notesStore.fetchNotes()
})

function viewNote(id: string) {
  router.push(`/notes/${id}`)
}

function editNote(id: string) {
  router.push(`/notes/${id}/edit`)
}

async function deleteNote(id: string) {
  const confirmed = confirm('Are you sure you want to delete this note?')
  if (confirmed) {
    await notesStore.deleteNote(id)
  }
}

function formatDate(dateStr: string) {
  return new Date(dateStr).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}
</script>

<template>
  <div class="mx-auto max-w-4xl px-4 py-8">
    <div class="mb-6 flex items-center justify-between">
      <h1 class="text-2xl font-bold text-gray-900">My Notes</h1>
      <div class="flex items-center gap-4">
        <span class="text-sm text-gray-500">{{ auth.token ? 'Logged in' : '' }}</span>
        <button
          @click="auth.logout(); router.push('/login')"
          class="rounded-md bg-gray-200 px-3 py-1.5 text-sm text-gray-700 hover:bg-gray-300"
        >
          Logout
        </button>
      </div>
    </div>

    <div class="mb-4 flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
      <input
        :value="notesStore.searchQuery"
        @input="notesStore.setSearchQuery(($event.target as HTMLInputElement).value)"
        type="text"
        placeholder="Search notes..."
        class="w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500 sm:w-72"
      />
      <div class="flex items-center gap-2">
        <span class="text-sm text-gray-500">Sort by:</span>
        <button
          @click="notesStore.setSortField('title')"
          class="rounded-md px-2.5 py-1.5 text-sm"
          :class="notesStore.sortField === 'title' ? 'bg-blue-100 text-blue-700' : 'bg-gray-100 text-gray-600 hover:bg-gray-200'"
        >
          Title {{ notesStore.sortField === 'title' ? (notesStore.sortDirection === 'asc' ? '↑' : '↓') : '' }}
        </button>
        <button
          @click="notesStore.setSortField('createdAt')"
          class="rounded-md px-2.5 py-1.5 text-sm"
          :class="notesStore.sortField === 'createdAt' ? 'bg-blue-100 text-blue-700' : 'bg-gray-100 text-gray-600 hover:bg-gray-200'"
        >
          Created {{ notesStore.sortField === 'createdAt' ? (notesStore.sortDirection === 'asc' ? '↑' : '↓') : '' }}
        </button>
        <button
          @click="notesStore.setSortField('updatedAt')"
          class="rounded-md px-2.5 py-1.5 text-sm"
          :class="notesStore.sortField === 'updatedAt' ? 'bg-blue-100 text-blue-700' : 'bg-gray-100 text-gray-600 hover:bg-gray-200'"
        >
          Updated {{ notesStore.sortField === 'updatedAt' ? (notesStore.sortDirection === 'asc' ? '↑' : '↓') : '' }}
        </button>
      </div>
    </div>

    <div class="mb-6">
      <router-link
        to="/notes/new"
        class="inline-block rounded-md bg-blue-600 px-4 py-2 text-white hover:bg-blue-700"
      >
        + New Note
      </router-link>
    </div>

    <div v-if="notesStore.loading" class="py-12 text-center text-gray-500">Loading...</div>

    <div v-else-if="notesStore.notes.length === 0" class="py-12 text-center text-gray-500">
      No notes found.
    </div>

    <div v-else class="space-y-3">
      <div
        v-for="note in notesStore.notes"
        :key="note.id"
        class="rounded-lg border border-gray-200 bg-white p-4 shadow-sm transition-shadow hover:shadow-md"
      >
        <div class="flex items-start justify-between">
          <div class="min-w-0 flex-1 cursor-pointer" @click="viewNote(note.id)">
            <h3 class="truncate text-lg font-semibold text-gray-900">{{ note.title }}</h3>
            <p class="mt-1 text-sm text-gray-500">{{ formatDate(note.createdAt) }}</p>
            <p v-if="note.content" class="mt-1 truncate text-sm text-gray-400">{{ note.content }}</p>
          </div>
          <div class="ml-4 flex shrink-0 gap-2">
            <button
              @click="editNote(note.id)"
              class="rounded-md px-2.5 py-1.5 text-sm text-gray-600 hover:bg-gray-100"
            >
              Edit
            </button>
            <button
              @click="deleteNote(note.id)"
              class="rounded-md px-2.5 py-1.5 text-sm text-red-600 hover:bg-red-50"
            >
              Delete
            </button>
          </div>
        </div>
      </div>
    </div>

    <div v-if="notesStore.totalPages > 1" class="mt-6 flex items-center justify-center gap-2">
      <button
        :disabled="notesStore.currentPage <= 1"
        @click="notesStore.fetchNotes(notesStore.currentPage - 1)"
        class="rounded-md px-3 py-1.5 text-sm disabled:opacity-50"
        :class="notesStore.currentPage > 1 ? 'bg-gray-100 hover:bg-gray-200' : 'bg-gray-50 text-gray-400'"
      >
        Previous
      </button>
      <span class="text-sm text-gray-600">
        Page {{ notesStore.currentPage }} of {{ notesStore.totalPages }}
      </span>
      <button
        :disabled="notesStore.currentPage >= notesStore.totalPages"
        @click="notesStore.fetchNotes(notesStore.currentPage + 1)"
        class="rounded-md px-3 py-1.5 text-sm disabled:opacity-50"
        :class="notesStore.currentPage < notesStore.totalPages ? 'bg-gray-100 hover:bg-gray-200' : 'bg-gray-50 text-gray-400'"
      >
        Next
      </button>
    </div>
  </div>
</template>
