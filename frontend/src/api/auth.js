const API_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000'

export async function login(login, password) {
    const response = await fetch(`${API_URL}/api/auth/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        credentials: 'include',
        body: JSON.stringify({ login, password }),
    })

    const data = await response.json()

    if (!response.ok) {
        throw new Error(data.message || 'Ошибка авторизации')
    }

    return data
}

export async function getCurrentUser() {
    const response = await fetch(`${API_URL}/api/auth/me`, {
        credentials: 'include',
    })

    if (!response.ok) {
        return null
    }

    return response.json()
}

export async function logout() {
    const response = await fetch(`${API_URL}/api/auth/logout`, {
        method: 'POST',
        credentials: 'include',
    })

    if (!response.ok) {
        throw new Error('Ошибка при выходе')
    }
}
