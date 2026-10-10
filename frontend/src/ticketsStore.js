import { ref } from 'vue'

export const items = ref([
	{
		id: 1,
		title: 'Не работает проектор',
		classroomId: 204,
		description: 'Проектор включается, но не выводит изображение.',
		status: 'New',
	},
	{
		id: 2,
		title: 'Проблема со звуком',
		classroomId: 108,
		description: 'Во время занятия отсутствует звук в колонках.',
		status: 'InProgress',
	},
	{
		id: 3,
		title: 'Неисправен интерактивный экран',
		classroomId: 310,
		description: 'Экран не реагирует на прикосновения.',
		status: 'Closed',
	},
])
