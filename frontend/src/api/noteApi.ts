import type { Note } from '@/types/Note'
import http from './api'
import type { PaginatedResponse } from '@/types/api'


export interface CreateNoteRequest {
  title: string
  content?: string
}

export interface UpdateNoteRequest {
  title?: string
  content?: string
}

export const notesApi = {
  list(page = 1, pageSize = 20, search?: string, sortBy?: string, sortDir?: string) {
    return http.get<PaginatedResponse<Note>>('/notes', {
      params: { page, pageSize, search, sortBy, sortDir },
    })
  },

  getById(id: string) {
    return http.get<Note>(`/notes/${id}`)
  },

  create(data: CreateNoteRequest) {
    return http.post<Note>('/notes', data)
  },

  update(id: string, data: UpdateNoteRequest) {
    return http.put<Note>(`/notes/${id}`, data)
  },

  delete(id: string) {
    return http.delete(`/notes/${id}`)
  },
}
