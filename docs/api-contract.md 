# API-контракт

## 1. Общие сведения

API реализуется на ASP.NET Core.

Базовый путь:

```text
/api/repair-requests
```

API предоставляет операции получения, создания, назначения исполнителя и изменения статуса заявки.

Удаление заявки через API не используется.

Для отмены используется статус `Cancelled`.

## 2. Таблица API

| Метод и путь                               | Тело                                  | Успех                                | Возможная ошибка |
| ------------------------------------------ | ------------------------------------- | ------------------------------------ | ---------------- |
| `GET /api/repair-requests`                 | —                                     | `200`, массив или `[]`               | —                |
| `GET /api/repair-requests/{id}`            | —                                     | `200`, объект                        | `404`            |
| `POST /api/repair-requests`                | `title`, `classroomId`, `description` | `201`, `id`, `number`, `status: New` | `400`            |
| `PATCH /api/repair-requests/{id}/assignee` | `assigneeUserId`                      | `200`                                | `404`            |
| `PATCH /api/repair-requests/{id}/status`   | `status`                              | `200`                                | `404`, `409`     |

## 3. GET списка заявок

```http
GET /api/repair-requests
```

Тело запроса отсутствует.

Успешный ответ:

```http
200 OK
```

Пример:

```json
[
  {
    "id": 1,
    "number": 1001,
    "title": "Не работает проектор",
    "description": "Проектор не включается.",
    "status": "New",
    "classroomId": 312,
    "createdByUserId": 5,
    "assigneeUserId": null
  }
]
```

Если заявок нет:

```http
200 OK
```

```json
[]
```

## 4. GET одной заявки

```http
GET /api/repair-requests/{id}
```

Пример:

```http
GET /api/repair-requests/1
```

Успех:

```http
200 OK
```

Пример ответа:

```json
{
  "id": 1,
  "number": 1001,
  "title": "Не работает проектор",
  "description": "Проектор не включается.",
  "status": "New",
  "classroomId": 312,
  "createdByUserId": 5,
  "assigneeUserId": null
}
```

Если заявки с таким `id` нет:

```http
404 Not Found
```

## 5. POST — создание заявки

```http
POST /api/repair-requests
```

Тело:

```json
{
  "title": "Не работает проектор",
  "classroomId": 312,
  "description": "Проектор не включается."
}
```

Клиент не передаёт:

* `id`;
* `number`;
* `status`;
* `assigneeUserId`.

Их определяет сервер.

Успех:

```http
201 Created
```

Пример:

```json
{
  "id": 1,
  "number": 1001,
  "title": "Не работает проектор",
  "description": "Проектор не включается.",
  "status": "New",
  "classroomId": 312,
  "createdByUserId": 5,
  "assigneeUserId": null
}
```

При создании:

```text
status = New
assigneeUserId = NULL
```

Если переданы некорректные данные:

```http
400 Bad Request
```

## 6. PATCH — назначение исполнителя

Назначение исполнителя является отдельным действием.

```http
PATCH /api/repair-requests/{id}/assignee
```

Пример:

```http
PATCH /api/repair-requests/1/assignee
```

Тело:

```json
{
  "assigneeUserId": 12
}
```

Успех:

```http
200 OK
```

После назначения:

```text
assigneeUserId = 12
status = New
```

То есть назначение исполнителя не переводит заявку в работу.

Если заявка не найдена:

```http
404 Not Found
```

## 7. PATCH — изменение статуса

Изменение статуса является отдельным действием.

```http
PATCH /api/repair-requests/{id}/status
```

Пример:

```http
PATCH /api/repair-requests/1/status
```

Тело:

```json
{
  "status": "InProgress"
}
```

Допустимые значения:

```text
New
InProgress
Closed
Cancelled
```

Успех:

```http
200 OK
```

Если заявка не найдена:

```http
404 Not Found
```

Если переход статуса запрещён правилами системы:

```http
409 Conflict
```

Например, назначенный техник может выполнить переход:

```text
New → InProgress
```

Другой техник не должен иметь возможность изменить эту заявку.

## 8. Правила API

1. `id` создаётся сервером.
2. `number` создаётся сервером.
3. Новая заявка получает статус `New`.
4. При создании исполнитель отсутствует.
5. Назначение исполнителя выполняется через отдельный `/assignee`.
6. Изменение статуса выполняется через отдельный `/status`.
7. Допустимые статусы: `New`, `InProgress`, `Closed`, `Cancelled`.
8. `DELETE` не используется.
9. Для отмены заявки используется `Cancelled`.
10. Путь `/create` не используется.
11. Проверка прав пользователя выполняется на сервере.
