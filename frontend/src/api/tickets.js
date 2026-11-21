const baseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000'

async function request(url, options = {}) {
    const response = await fetch(`${baseUrl}${url}`, {
        credentials: 'include',
        ...options,
        headers: {
            ...(options.headers || {}),
        },
    })

    if (!response.ok) {
        const error = new Error(
            response.status === 401 ? 'Необходима авторизация' :
            response.status === 403 ? 'Недостаточно прав' :
            response.status === 400 ? 'Проверьте введённые данные' :
            'Ошибка API'
        )
        error.status = response.status
        throw error
    }

    if (response.status === 204) return null
    return response.json()
}

function normalize(item) {
    return {
        ...item,
        siteId: item.classroomId,
        number: String(item.number),
    }
}

export async function getTickets() {
    const data = await request('/api/repair-requests')
    return data.map(normalize)
}

export async function createTicket(body) {
    const created = await request('/api/repair-requests', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            title: body.title,
            classroomId: Number(body.siteId),
            description: body.description,
        }),
    })
    return normalize(created)
}

export async function assignTicket(id, assigneeUserId) {
    return normalize(await request(`/api/repair-requests/${id}/assignee`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ assigneeUserId }),
    }))
}

export async function updateTicketStatus(id, status) {
    return normalize(await request(`/api/repair-requests/${id}/status`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ status }),
    }))
}
