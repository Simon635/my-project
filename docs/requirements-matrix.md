# Матрица требований

## 1. Связь требований ЛР1 с моделью и API

В ЛР1 определены три основных критерия приёмки:

1. создать заявку;
2. назначить исполнителя;
3. перевести заявку в работу.

Каждому критерию соответствует поле модели данных и HTTP-запрос API.

## 2. Основная матрица

| Требование ЛР1            | Поле или сущность                                      | Запрос                                     | Критерий |
| ------------------------- | ------------------------------------------------------ | ------------------------------------------ | -------- |
| Создать заявку            | `RepairRequest`, `title`, `classroomId`, `description` | `POST /api/repair-requests`                | 1        |
| Назначить исполнителя     | `assigneeUserId`                                       | `PATCH /api/repair-requests/{id}/assignee` | 2        |
| Перевести заявку в работу | `status`                                               | `PATCH /api/repair-requests/{id}/status`   | 3        |

## 3. Критерий 1 — создать заявку

### Требование ЛР1

Преподаватель может создать заявку с заголовком, аудиторией и описанием.

### Поля модели

```text
RepairRequest.title
RepairRequest.classroomId
RepairRequest.description
```

После создания сервер устанавливает:

```text
status = New
```

Также сервер создаёт:

```text
id
number
```

### Запрос

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

Результат:

```text
201 Created
```

## 4. Критерий 2 — назначить исполнителя

### Требование ЛР1

Диспетчер отдельно назначает техника. После назначения исполнитель появляется в заявке, а статус остаётся `New`.

### Поле модели

```text
RepairRequest.assigneeUserId
```

Внешний ключ:

```text
RepairRequest.assigneeUserId → User.id
```

До назначения:

```text
assigneeUserId = NULL
```

После назначения:

```text
assigneeUserId = id техника
```

### Запрос

```http
PATCH /api/repair-requests/{id}/assignee
```

Тело:

```json
{
  "assigneeUserId": 12
}
```

Результат:

```text
200 OK
```

Статус не изменяется:

```text
New → New
```

## 5. Критерий 3 — исполнитель перевёл заявку в работу

### Требование ЛР1

Назначенный техник может перевести свою заявку из `New` в `InProgress`.

Другой техник не может изменить эту заявку.

### Поле модели

```text
RepairRequest.status
```

Исполнитель определяется через:

```text
RepairRequest.assigneeUserId
```

### Запрос

```http
PATCH /api/repair-requests/{id}/status
```

Тело:

```json
{
  "status": "InProgress"
}
```

Результат:

```text
200 OK
```

Если переход запрещён:

```text
409 Conflict
```

Проверка того, что текущий пользователь является назначенным исполнителем заявки, выполняется на ASP.NET Core.

## 6. Дополнительное соответствие требованиям ЛР1

| Требование                         | Поле / сущность              | API                         |
| ---------------------------------- | ---------------------------- | --------------------------- |
| Заявка получает человеческий номер | `RepairRequest.number`       | `POST /api/repair-requests` |
| Новая заявка получает статус `New` | `RepairRequest.status`       | `POST /api/repair-requests` |
| Хранится автор заявки              | `createdByUserId → User.id`  | `POST /api/repair-requests` |
| Хранится аудитория                 | `classroomId → Classroom.id` | `GET /api/repair-requests`  |
| Хранится исполнитель               | `assigneeUserId → User.id`   | `PATCH .../assignee`        |
| Техник не меняет чужую заявку      | `assigneeUserId`             | `PATCH .../status`          |
| Диспетчер может отменить заявку    | `status = Cancelled`         | `PATCH .../status`          |

## 7. Итоговая связь

```text
Критерий 1
Создать заявку
        ↓
title + classroomId + description
        ↓
POST /api/repair-requests

Критерий 2
Назначить исполнителя
        ↓
assigneeUserId
        ↓
PATCH /api/repair-requests/{id}/assignee

Критерий 3
Перевести в работу
        ↓
status
        ↓
PATCH /api/repair-requests/{id}/status
```

Все три критерия ЛР1 имеют соответствующее поле или сущность модели данных и соответствующий HTTP-запрос API.
