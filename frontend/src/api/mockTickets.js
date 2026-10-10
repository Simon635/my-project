const delay = (ms = 500) =>
	new Promise(resolve => setTimeout(resolve, ms))

let nextId = 4

let mockItems = [
	{
		id: 1,
		number: 'TMP-1',
		title: 'Не работает проектор',
		siteId: 2,
		description: 'Проектор включается, но не выводит изображение.',
		status: 'New',
	},
	{
		id: 2,
		number: 'TMP-2',
		title: 'Проблема со звуком',
		siteId: 1,
		description: 'Во время занятия отсутствует звук в колонках.',
		status: 'InProgress',
	},
	{
		id: 3,
		number: 'TMP-3',
		title: 'Неисправен интерактивный экран',
		siteId: 3,
		description: 'Экран не реагирует на прикосновения.',
		status: 'Closed',
	},
]

export async function mockGetTickets() {
	await delay()

	if (import.meta.env.VITE_MOCK_FORCE_500 === 'true') {
		return {
			ok: false,
			status: 500,
			data: { message: 'Учебная ошибка сервера' },
		}
	}

	return {
		ok: true,
		status: 200,
		data: [...mockItems],
	}
}

export async function mockCreateTicket(body) {
	await delay()

	if (import.meta.env.VITE_MOCK_FORCE_500 === 'true') {
		return {
			ok: false,
			status: 500,
			data: { message: 'Учебная ошибка сервера' },
		}
	}

	if (
		!body.title ||
		body.title.trim().length < 5 ||
		body.title.trim().length > 80 ||
		!Number.isInteger(body.siteId) ||
		body.siteId <= 0 ||
		!body.description ||
		body.description.trim().length < 10 ||
		body.description.trim().length > 500
	) {
		return {
			ok: false,
			status: 400,
			data: { message: 'Проверьте поля заявки' },
		}
	}

	const item = {
		id: nextId,
		number: `TMP-${nextId}`,
		title: body.title.trim(),
		siteId: body.siteId,
		description: body.description.trim(),
		status: 'New',
	}

	nextId += 1
	mockItems.unshift(item)

	return {
		ok: true,
		status: 201,
		data: item,
	}
}
