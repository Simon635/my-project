-- Тестовые данные для лабораторной работы №9

INSERT INTO classrooms (id, name) VALUES
    (1, 'Аудитория 205'),
    (2, 'Аудитория 210'),
    (3, 'Аудитория 215');

INSERT INTO users (id, login, name, role) VALUES
    (5, 'teacher01', 'Иванов Иван', 'Teacher'),
    (6, 'dispatcher01', 'Петров Пётр', 'Dispatcher'),
    (7, 'technician01', 'Сидоров Алексей', 'Technician');

INSERT INTO repair_requests
    (number, title, description, status, classroom_id,
     created_by_user_id, assignee_user_id, created_at)
VALUES
    (1001, 'Не работает проектор',
     'Проектор не включается после подачи питания',
     'InProgress', 1, 5, 7, CURRENT_TIMESTAMP - INTERVAL '3 days'),

    (1002, 'Не работает микрофон',
     'Микрофон не передает звук на акустическую систему',
     'New', 2, 5, NULL, CURRENT_TIMESTAMP - INTERVAL '2 days'),

    (1003, 'Неисправна акустика',
     'В колонках отсутствует звук',
     'Closed', 3, 5, 7, CURRENT_TIMESTAMP - INTERVAL '5 days'),

    (1004, 'Проблема с экраном',
     'Экран проектора не опускается',
     'Cancelled', 1, 6, NULL, CURRENT_TIMESTAMP - INTERVAL '1 day');
