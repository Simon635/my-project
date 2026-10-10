<script setup>
import { reactive, ref } from 'vue'

const form = reactive({
  email: '',
  password: '',
})

const loginMessage = ref('')

function login() {
  loginMessage.value =
    'Форма проверена. Авторизация будет подключена после появления backend.'
}
</script>

<template>
  <main class="main">
    <section class="page-heading">
      <div>
        <p class="eyebrow">ЛИЧНЫЙ КАБИНЕТ</p>
        <h1>Вход в систему</h1>
        <p>Войдите, чтобы работать с заявками на ремонт.</p>
      </div>
    </section>

    <section class="login-section">
      <div class="login-icon">М</div>

      <form class="request-form" @submit.prevent="login">
        <div>
          <label for="email">Электронная почта</label>
          <input
            id="email"
            v-model="form.email"
            type="email"
            placeholder="name@example.com"
            autocomplete="username"
            required
          />
        </div>

        <div>
          <label for="password">Пароль</label>
          <input
            id="password"
            v-model="form.password"
            type="password"
            placeholder="Введите пароль"
            autocomplete="current-password"
            required
          />
        </div>

        <button type="submit" class="primary-button login-button">
          Войти
        </button>

        <p v-if="loginMessage" class="login-message">
          {{ loginMessage }}
        </p>
      </form>

      <RouterLink to="/" class="back-link">
        Вернуться к заявкам
      </RouterLink>
    </section>
  </main>
</template>
