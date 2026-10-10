<script setup>
import { items } from '../ticketsStore'
import TicketTable from '../components/TicketTable.vue'

const statusLabels = {
  New: 'Новые',
  InProgress: 'В работе',
  Closed: 'Закрытые',
  Cancelled: 'Отменённые',
}
</script>

<template>
  <main class="main">
    <section class="page-heading">
      <div>
        <p class="eyebrow">СИСТЕМА УЧЁТА ОБРАЩЕНИЙ</p>
        <h1>Заявки на ремонт</h1>
        <p>Управляйте обращениями по ремонту мультимедийного оборудования.</p>
      </div>
    </section>

    <section class="summary">
      <div class="summary-card">
        <span>Всего заявок</span>
        <strong>{{ items.length }}</strong>
      </div>

      <div class="summary-card">
        <span>Новые</span>
        <strong>{{ items.filter(item => item.status === 'New').length }}</strong>
      </div>

      <div class="summary-card">
        <span>В работе</span>
        <strong>{{ items.filter(item => item.status === 'InProgress').length }}</strong>
      </div>

      <div class="summary-card">
        <span>Закрытые</span>
        <strong>{{ items.filter(item => item.status === 'Closed').length }}</strong>
      </div>
    </section>

    <section class="list-heading">
      <div>
        <h2>Список обращений</h2>
        <p>Все зарегистрированные заявки</p>
      </div>

      <RouterLink to="/new" class="primary-button">
        + Создать заявку
      </RouterLink>
    </section>

    <TicketTable :items="items" />
  </main>
</template>
