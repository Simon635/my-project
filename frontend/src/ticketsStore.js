import { ref } from 'vue'

export const items = ref([
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
])

let nextId = Math.max(...items.value.map(item => item.id)) + 1

export function addItem({ title, siteId, description }) {
	const item = {
		id: nextId,
		number: `TMP-${nextId}`,
		title,
		siteId,
		description,
		status: 'New',
	}

	nextId += 1
	items.value.unshift(item)

	return item
}
