-- Лабораторная работа №9
-- Примеры SQL-запросов к базе данных системы ремонта

-- 1. Фильтрация и сортировка:
-- Получить новые заявки, начиная с самых свежих.
SELECT
    number,
    title,
    status,
    created_at
FROM repair_requests
WHERE status = 'New'
ORDER BY created_at DESC;

-- 2. JOIN:
-- Получить заявки с названием аудитории,
-- именем автора и именем назначенного исполнителя.
SELECT
    rr.number,
    rr.title,
    rr.status,
    c.name AS classroom,
    creator.name AS created_by,
    assignee.name AS assignee
FROM repair_requests AS rr
JOIN classrooms AS c
    ON rr.classroom_id = c.id
JOIN users AS creator
    ON rr.created_by_user_id = creator.id
LEFT JOIN users AS assignee
    ON rr.assignee_user_id = assignee.id
ORDER BY rr.number;

-- 3. GROUP BY:
-- Подсчитать количество заявок в каждом статусе.
SELECT
    status,
    COUNT(*) AS request_count
FROM repair_requests
GROUP BY status
ORDER BY status;
