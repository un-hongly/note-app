<script setup lang="ts">
import type { Note } from '@/types/Note'
import { formatDate } from '@/utils/helpers'

defineProps<{ note: Note }>()
const emit = defineEmits<{
  view: [id: string]
  edit: [id: string]
  delete: [id: string]
}>()
</script>

<template>
  <div class="rounded-lg border border-gray-200 bg-white p-4 shadow-sm transition-shadow hover:shadow-md">
    <div class="flex items-start justify-between">
      <div class="min-w-0 flex-1 cursor-pointer" @click="emit('view', note.id)">
        <h3 class="truncate text-lg font-semibold text-gray-900">{{ note.title }}</h3>
        <p class="mt-1 text-sm text-gray-500">{{ formatDate(note.createdAt) }}</p>
        <p v-if="note.content" class="mt-1 truncate text-sm text-gray-400">{{ note.content }}</p>
      </div>
      <div class="ml-4 flex shrink-0 gap-2">
        <button
          @click="emit('edit', note.id)"
          class="rounded-md px-2.5 py-1.5 text-sm text-gray-600 hover:bg-gray-100"
        >
          Edit
        </button>
        <button
          @click="emit('delete', note.id)"
          class="rounded-md px-2.5 py-1.5 text-sm text-red-600 hover:bg-red-50"
        >
          Delete
        </button>
      </div>
    </div>
  </div>
</template>
