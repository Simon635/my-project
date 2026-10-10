import {
	mockGetTickets,
	mockCreateTicket,
} from './mockTickets'

// true — используем учебный mock
// false — отправляем настоящие HTTP-запросы
const USE_MOCK = true

const baseUrl = import.meta.env.VITE_API_BASE_URL

function handleMockResponse(response) {
	if (!response.ok) {
		const error = new Error(
			response.status === 400
				? 'Проверьте поля заявки'
				: 'Сервер не смог обработать запрос',
		)

		error.status = response.status
		throw error
	}

	return response.data
}

async function handleFetchResponse(response) {
	if (!response.ok) {
		const error = new Error(
			response.status === 400
				? 'Проверьте поля заявки'
				: 'Сервер не смог обработать запрос',
		)

		error.status = response.status
		throw error
	}

	return response.json()
}

export async function getTickets() {
	if (USE_MOCK) {
		const response = await mockGetTickets()
		return handleMockResponse(response)
	}

	try {
		const response = await fetch(`${baseUrl}/api/tickets`)
		return await handleFetchResponse(response)
	} catch (error) {
		if (error.status) throw error

		throw new Error('Нет связи с сервером')
	}
}

export async function createTicket(body) {
	if (USE_MOCK) {
		const response = await mockCreateTicket(body)
		return handleMockResponse(response)
	}

	try {
		const response = await fetch(`${baseUrl}/api/tickets`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify(body),
		})

		return await handleFetchResponse(response)
	} catch (error) {
		if (error.status) throw error

		throw new Error('Нет связи с сервером')
	}
}
