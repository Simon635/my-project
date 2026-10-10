<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { addItem } from '../ticketsStore'

const router = useRouter()

const form = reactive({
  title: '',
  siteId: 0,
  description: '',
})

const errors = reactive({
  title: '',
  siteId: '',
  description: '',
})

function onCreate() {
  errors.title = ''
  errors.siteId = ''
  errors.description = ''

  const title = form.title.trim()
  const description = form.description.trim()
  const siteId = form.siteId

  let isValid = true

  if (title.length < 5 || title.length > 80) {
    errors.title = 'Тема: от 5 до 80 символов.'
    isValid = false
  }

  if (!Number.isInteger(siteId) || siteId <= 0) {
    errors.siteId = 'Укажите целое число больше нуля.'
    isValid = false
  }

  if (description.length < 10 || description.length > 500) {
    errors.description = 'Описание: от 10 до 500 символов.'
    isValid = false
  }

  if (!isValid) return

  addItem({
    title,
    siteId,
    description,
  })

  form.title = ''
  form.siteId = 0
  form.description = ''

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
      <form class="request-form" @submit.prevent="onCreate">
        <div>
          <label for="title">Название заявки</label>
          <input
            id="title"
            v-model="form.title"
            type="text"
            minlength="5"
            maxlength="80"
            placeholder="Например, не работает проектор"
            autofocus
            required
          />
          <p class="form-hint">От 5 до 80 символов.</p>
          <p v-if="errors.title" class="field-error" role="alert">
            {{ errors.title }}
          </p>
        </div>

        <div>
          <label for="siteId">ID площадки</label>
          <input
            id="siteId"
            v-model.number="form.siteId"
            type="number"
            min="1"
            step="1"
            required
          />
          <p v-if="errors.siteId" class="field-error" role="alert">
            {{ errors.siteId }}
          </p>
        </div>

        <div>
          <label for="description">Описание неисправности</label>
          <textarea
            id="description"
            v-model="form.description"
            minlength="10"
            maxlength="500"
            rows="5"
            placeholder="Опишите неисправность оборудования"
            required
          />
          <p class="form-hint">От 10 до 500 символов.</p>
          <p v-if="errors.description" class="field-error" role="alert">
            {{ errors.description }}
          </p>
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
