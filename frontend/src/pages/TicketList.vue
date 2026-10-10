<script setup>
import { computed, onMounted, ref } from 'vue'
import TicketTable from '../components/TicketTable.vue'
import { getTickets } from '../api/tickets'
import { items } from '../ticketsStore'

const screen = ref('loading')
const errorMessage = ref('')

const totalCount = computed(() => items.value.length)
const newCount = computed(
  () => items.value.filter(item => item.status === 'New').length,
)
const inProgressCount = computed(
  () => items.value.filter(item => item.status === 'InProgress').length,
)
const closedCount = computed(
  () => items.value.filter(item => item.status === 'Closed').length,
)

async function loadTickets() {
  screen.value = 'loading'
  errorMessage.value = ''

  try {
    const data = await getTickets()
    items.value = data
    screen.value = data.length === 0 ? 'empty' : 'data'
  } catch (error) {
    errorMessage.value = error.message || 'Не удалось загрузить заявки'
    screen.value = 'error'
  }
}

onMounted(loadTickets)
</script>

<template>
  <main class="main">
    <section class="page-heading">
      <div>
        <p class="eyebrow">УЧЁТ НЕИСПРАВНОСТЕЙ</p>
        <h1>Заявки на ремонт</h1>
        <p>Список обращений по мультимедийному оборудованию.</p>
      </div>

      <RouterLink to="/new" class="primary-button">
        Создать заявку
      </RouterLink>
    </section>

    <section class="stats-grid">
      <article class="stat-card">
        <span class="stat-label">Всего заявок</span>
        <strong>{{ totalCount }}</strong>
      </article>

      <article class="stat-card">
        <span class="stat-label">Новые</span>
        <strong>{{ newCount }}</strong>
      </article>

      <article class="stat-card">
        <span class="stat-label">В работе</span>
        <strong>{{ inProgressCount }}</strong>
      </article>

      <article class="stat-card">
        <span class="stat-label">Закрытые</span>
        <strong>{{ closedCount }}</strong>
      </article>
    </section>

    <section class="list-section">
      <div class="section-heading">
        <div>
          <h2>Все обращения</h2>
          <p>Заявки загружаются через API-модуль.</p>
        </div>

        <button
          type="button"
          class="secondary-button"
          @click="loadTickets"
        >
          Обновить список
        </button>
      </div>

      <p v-if="screen === 'loading'" role="status">
        Загрузка…
      </p>

      <div v-else-if="screen === 'empty'">
        Записей пока нет.
      </div>

      <div v-else-if="screen === 'error'" role="alert">
        {{ errorMessage }}
        <button
          type="button"
          class="secondary-button"
          @click="loadTickets"
        >
          Повторить
        </button>
      </div>

      <TicketTable v-else :items="items" />
    </section>
  </main>
</template>
