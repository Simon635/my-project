<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { createTicket, getTickets } from '../api/tickets'
import { items } from '../ticketsStore'

const router = useRouter()

const form = reactive({
  title: '',
  siteId: '',
  description: '',
})

const errors = reactive({
  title: '',
  siteId: '',
  description: '',
})

const isSubmitting = ref(false)
const submitError = ref('')

function validateForm() {
  errors.title = ''
  errors.siteId = ''
  errors.description = ''

  const title = form.title.trim()
  const description = form.description.trim()
  const siteId = Number(form.siteId)

  if (title.length < 5 || title.length > 80) {
    errors.title = 'Тема: от 5 до 80 символов.'
  }

  if (!Number.isInteger(siteId) || siteId <= 0) {
    errors.siteId = 'Укажите целое число больше нуля.'
  }

  if (description.length < 10 || description.length > 500) {
    errors.description = 'Описание: от 10 до 500 символов.'
  }

  return {
    valid: !errors.title && !errors.siteId && !errors.description,
    title,
    siteId,
    description,
  }
}

async function onCreate() {
  if (isSubmitting.value) return

  submitError.value = ''

  const result = validateForm()
  if (!result.valid) return

  isSubmitting.value = true

  try {
    const createdItem = await createTicket({
      title: result.title,
      siteId: result.siteId,
      description: result.description,
    })

    // Обновляем общий список данными из API.
    // Если повторная загрузка не удалась, всё равно сохраняем
    // полученную от API созданную запись в текущем интерфейсе.
    try {
      const data = await getTickets()
      items.value = data
    } catch {
      items.value.unshift(createdItem)
    }

    form.title = ''
    form.siteId = ''
    form.description = ''

    await router.push('/')
  } catch (error) {
    submitError.value = error.message || 'Не удалось создать заявку'
  } finally {
    isSubmitting.value = false
  }
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
      <form
        class="request-form"
        novalidate
        @submit.prevent="onCreate"
      >
        <div>
          <label for="title">Название заявки</label>
          <input
            id="title"
            v-model="form.title"
            type="text"
            maxlength="80"
            placeholder="Например, не работает проектор"
            required
            :aria-invalid="Boolean(errors.title)"
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
            :aria-invalid="Boolean(errors.siteId)"
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
            :aria-invalid="Boolean(errors.description)"
          />
          <p class="form-hint">От 10 до 500 символов.</p>
          <p v-if="errors.description" class="field-error" role="alert">
            {{ errors.description }}
          </p>
        </div>

        <p v-if="submitError" class="field-error" role="alert">
          {{ submitError }}
        </p>

        <div class="form-actions">
          <RouterLink to="/" class="secondary-button">
            Отмена
          </RouterLink>

          <button
            type="submit"
            class="primary-button"
            :disabled="isSubmitting"
          >
            {{ isSubmitting ? 'Создаём…' : 'Создать заявку' }}
          </button>
        </div>
      </form>
    </section>
  </main>
</template>
