<script setup>
import { useRoute } from 'vue-router'

const route = useRoute()
</script>

<template>
  <header class="header">
    <div class="header-content">
      <RouterLink to="/" class="brand">
        <span class="brand-icon">М</span>
        <span>МедиаРемонт</span>
      </RouterLink>

      <nav class="navigation">
        <RouterLink
          to="/"
          class="nav-link"
          :class="{ active: route.path === '/' }"
        >
          Заявки
        </RouterLink>

        <RouterLink
          to="/new"
          class="nav-link"
          :class="{ active: route.path === '/new' }"
        >
          Создать заявку
        </RouterLink>

        <RouterLink
          to="/login"
          class="nav-link"
          :class="{ active: route.path === '/login' }"
        >
          Войти
        </RouterLink>
      </nav>
    </div>
  </header>
</template>
