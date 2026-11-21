<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { login } from '../api/auth'

const router = useRouter()

const form = reactive({
    login: '',
    password: '',
})

const loginMessage = ref('')
const isLoading = ref(false)

async function handleLogin() {
    loginMessage.value = ''
    isLoading.value = true

    try {
        await login(form.login, form.password)
        router.push('/')
    } catch (error) {
        loginMessage.value = error.message
    } finally {
        isLoading.value = false
    }
}
</script>

<template>
    <section class="login">
        <h1>Вход в систему</h1>

        <form @submit.prevent="handleLogin">
            <label>
                Логин
                <input
                    v-model="form.login"
                    type="text"
                    autocomplete="username"
                    required
                />
            </label>

            <label>
                Пароль
                <input
                    v-model="form.password"
                    type="password"
                    autocomplete="current-password"
                    required
                />
            </label>

            <button type="submit" :disabled="isLoading">
                {{ isLoading ? 'Вход...' : 'Войти' }}
            </button>
        </form>

        <p v-if="loginMessage" class="login__error">
            {{ loginMessage }}
        </p>
    </section>
</template>
