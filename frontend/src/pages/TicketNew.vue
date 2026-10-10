<script setup>
import { reactive } from 'vue'
import { useRouter } from 'vue-router'
import { items } from '../ticketsStore'

const router = useRouter()

const form = reactive({
  title: '',
  classroomId: '',
  description: '',
})

function createRequest() {
  const title = form.title.trim()
  const description = form.description.trim()
  const classroomId = Number(form.classroomId)

  if (title.length < 5 || title.length > 80) {
    alert('Название должно содержать от 5 до 80 символов.')
    return
  }

  if (description.length < 10 || description.length > 500) {
    alert('Описание должно содержать от 10 до 500 символов.')
    return
  }

  if (!Number.isInteger(classroomId) || classroomId < 1) {
    alert('Введите корректный номер аудитории.')
    return
  }

  const nextId = items.value.length
    ? Math.max(...items.value.map(item => item.id)) + 1
    : 1

  items.value.unshift({
    id: nextId,
    title,
    classroomId,
    description,
    status: 'New',
  })

  router.push('/')
}
</script>

<template>
  <main class="main">
    <section class="page-heading">
      <div>
        <p class="eyebrow">НОВОЕ ОБРАЩЕНИЕ</p>
        <h1>Создать заявку</h1>
        <p>Опишите неисправность мультимедийного оборудования.</p>
      </div>
    </section>

    <section class="form-section">
      <form class="request-form" @submit.prevent="createRequest">
        <div>
          <label for="title">Название заявки</label>
          <input
            id="title"
            v-model="form.title"
            type="text"
            minlength="5"
            maxlength="80"
            placeholder="Например, не работает проектор"
            required
          />
          <p class="form-hint">От 5 до 80 символов.</p>
        </div>

        <div>
          <label for="classroomId">Номер аудитории</label>
          <input
            id="classroomId"
            v-model="form.classroomId"
            type="number"
            min="1"
            step="1"
            placeholder="Например, 204"
            required
          />
        </div>

        <div>
          <label for="description">Описание неисправности</label>
          <textarea
            id="description"
            v-model="form.description"
            minlength="10"
            maxlength="500"
            rows="5"
            placeholder="Опишите, что произошло и какое оборудование неисправно"
            required
          />
          <p class="form-hint">От 10 до 500 символов.</p>
        </div>

        <div class="form-actions">
          <RouterLink to="/" class="secondary-button">
            Отмена
          </RouterLink>

          <button type="submit" class="primary-button">
            Создать заявку
          </button>
        </div>
      </form>
    </section>
  </main>
</template>
