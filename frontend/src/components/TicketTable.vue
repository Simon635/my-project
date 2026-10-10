<script setup>
defineProps({
  items: {
    type: Array,
    required: true,
  },
})

const statusLabels = {
  New: 'Новая',
  InProgress: 'В работе',
  Closed: 'Закрыта',
  Cancelled: 'Отменена',
}
</script>

<template>
  <div v-if="items.length" class="request-list">
    <article
      v-for="item in items"
      :key="item.id"
      class="request-card"
    >
      <div class="request-main">
        <div class="request-icon">🔧</div>

        <div class="request-info">
          <div class="request-title-row">
            <h3 class="request-title">{{ item.title }}</h3>

            <span
              class="status"
              :class="`status-${item.status.toLowerCase()}`"
            >
              {{ statusLabels[item.status] || item.status }}
            </span>
          </div>

          <p>{{ item.description }}</p>

          <div class="request-meta">
            <span>Аудитория: {{ item.classroomId }}</span>
            <span>№ {{ item.id }}</span>
          </div>
        </div>
      </div>
    </article>
  </div>

  <div v-else class="empty-state">
    <h3>Заявок пока нет</h3>
    <p>Создайте первую заявку на ремонт оборудования.</p>
  </div>
</template>
