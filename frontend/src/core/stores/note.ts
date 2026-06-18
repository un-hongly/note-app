import { ref } from 'vue'
import { defineStore } from 'pinia'
import { notesApi, type CreateNoteRequest, type UpdateNoteRequest } from '@/core/api/noteApi'
import type { Note } from '@/core/models/Note'

export type SortField = 'title' | 'createdAt' | 'updatedAt'
export type SortDirection = 'asc' | 'desc'

export const useNotesStore = defineStore('notes', () => {
  const notes = ref<Note[]>([])
  const loading = ref(false)
  const totalCount = ref(0)
  const currentPage = ref(1)
  const pageSize = ref(5)
  const totalPages = ref(0)

  const searchQuery = ref('')
  const sortField = ref<SortField>('createdAt')
  const sortDirection = ref<SortDirection>('desc')

  let searchTimeout: ReturnType<typeof setTimeout> | null = null

  function setSortField(field: SortField) {
    if (sortField.value === field) {
      sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc'
    } else {
      sortField.value = field
      sortDirection.value = 'asc'
    }
    fetchNotes()
  }

  function setSearchQuery(query: string) {
    searchQuery.value = query
    if (searchTimeout) clearTimeout(searchTimeout)
    searchTimeout = setTimeout(() => {
      currentPage.value = 1
      fetchNotes()
    }, 400)
  }

  const fetchNotes = async (page?: number) => {
    loading.value = true
    try {
      const res = await notesApi.list(
        page ?? currentPage.value,
        pageSize.value,
        searchQuery.value || undefined,
        sortField.value,
        sortDirection.value,
      )
      notes.value = res.data.items
      totalCount.value = res.data.totalCount
      currentPage.value = res.data.page
      totalPages.value = res.data.totalPages
    } finally {
      loading.value = false
    }
  }

  const fetchNoteById = async (id: string) => {
    const res = await notesApi.getById(id)
    return res.data
  }

  const createNote = async (data: CreateNoteRequest) => {
    await notesApi.create(data)
    await fetchNotes()
  }

  const updateNote = async (id: string, data: UpdateNoteRequest) => {
    await notesApi.update(id, data)
    await fetchNotes()
  }

  const deleteNote = async (id: string) => {
    await notesApi.delete(id)
    await fetchNotes()
  }

  return {
    notes,
    loading,
    totalCount,
    currentPage,
    pageSize,
    totalPages,
    searchQuery,
    sortField,
    sortDirection,
    setSortField,
    setSearchQuery,
    fetchNotes,
    fetchNoteById,
    createNote,
    updateNote,
    deleteNote,
  }
})
