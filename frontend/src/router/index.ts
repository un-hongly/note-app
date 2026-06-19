import { createRouter, createWebHashHistory, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHashHistory(),
  // history: createWebHistory(),
  routes: [
    {
      path: '/',
      redirect: '/notes',
    },
    {
      path: '/login',
      component: () => import('@/views/auth/LoginPage.vue'),
    },
    {
      path: '/register',
      component: () => import('@/views/auth/RegisterPage.vue'),
    },
    {
      path: '/notes',
      component: () => import('@/views/note/NotesListPage.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/notes/new',
      component: () => import('@/views/note/NoteFormPage.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/notes/:id',
      component: () => import('@/views/note/NoteDetailPage.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/notes/:id/edit',
      component: () => import('@/views/note/NoteFormPage.vue'),
      meta: { requiresAuth: true },
    },
  ],
})

router.beforeEach((to, _from, next) => {
  const token = localStorage.getItem('accessToken')
  if (to.meta.requiresAuth && !token) {
    next({ path: '/login' })
  } else if ((to.path === '/login' || to.path === '/register') && token) {
    next({ path: '/notes' })
  } else {
    next()
  }
})

export default router
