INSERT INTO class_rooms (name, grade_level, section, academic_year)
VALUES 
    ('Grade 9-A', 9, 'A', 2026),
    ('Grade 9-B', 9, 'B', 2026),
    ('Grade 10-A', 10, 'A', 2026),
    ('Grade 10-B', 10, 'B', 2026),
    ('Grade 11-A', 11, 'A', 2026);

INSERT INTO teachers (employee_code, full_name, email, phone, subject, hire_date, salary)
VALUES 
    (101, 'Dr. Alan Turing', 'alan.turing@school.edu', '03001000001', 'Computer Science', '2023-08-15', 85000.00),
    (102, 'Marie Curie', 'marie.curie@school.edu', '03001000002', 'Physics', '2022-01-10', 90000.00),
    (103, 'Isaac Newton', 'isaac.newton@school.edu', '03001000003', 'Mathematics', '2021-09-01', 95000.00),
    (104, 'Ada Lovelace', 'ada.lovelace@school.edu', '03001000004', 'Algorithms', '2024-02-01', 88000.00);

INSERT INTO courses (code, title, credit_hours, capacity, teacher_id)
VALUES 
    (
        'CS101', 
        'Introduction to Computer Science', 
        4, 
        30, 
        (SELECT id FROM teachers WHERE employee_code = 101)
    ),
    (
        'PHY101', 
        'General Physics I', 
        3, 
        35, 
        (SELECT id FROM teachers WHERE employee_code = 102)
    ),
    (
        'MATH101', 
        'Calculus and Analytical Geometry', 
        4, 
        40, 
        (SELECT id FROM teachers WHERE employee_code = 103)
    ),
    (
        'CS201', 
        'Data Structures and Algorithms', 
        4, 
        25, 
        (SELECT id FROM teachers WHERE employee_code = 104)
    );

INSERT INTO students (roll_number, first_name, last_name, email, class_room_id, enrolled_on)
VALUES 
    (
        'F26-001', 
        'Alice', 
        'Smith', 
        'alice.smith@student.school.edu', 
        (SELECT id FROM class_rooms WHERE name = 'Grade 10-A'), 
        '2026-01-15'
    ),
    (
        'F26-002', 
        'Bob', 
        'Johnson', 
        'bob.johnson@student.school.edu', 
        (SELECT id FROM class_rooms WHERE name = 'Grade 10-A'), 
        '2026-01-15'
    ),
    (
        'F26-003', 
        'Charlie', 
        'Brown', 
        'charlie.brown@student.school.edu', 
        (SELECT id FROM class_rooms WHERE name = 'Grade 10-B'), 
        '2026-01-16'
    ),
    (
        'F26-004', 
        'Diana', 
        'Prince', 
        'diana.prince@student.school.edu', 
        (SELECT id FROM class_rooms WHERE name = 'Grade 9-A'), 
        '2026-01-16'
    ),
    (
        'F26-005', 
        'Edward', 
        'Elric', 
        'edward.elric@student.school.edu', 
        (SELECT id FROM class_rooms WHERE name = 'Grade 11-A'), 
        '2026-01-17'
    );

INSERT INTO enrollments (student_id, course_id, enrolled_on, marks, grade)
VALUES 
    (
        (SELECT id FROM students WHERE roll_number = 'F26-001'), 
        (SELECT id FROM courses WHERE code = 'CS101'), 
        '2026-01-20', 
        92, 
        'A'
    ),
    (
        (SELECT id FROM students WHERE roll_number = 'F26-001'), 
        (SELECT id FROM courses WHERE code = 'MATH101'), 
        '2026-01-20', 
        85, 
        'B'
    ),
    (
        (SELECT id FROM students WHERE roll_number = 'F26-002'), 
        (SELECT id FROM courses WHERE code = 'CS101'), 
        '2026-01-21', 
        78, 
        'C'
    ),
    (
        (SELECT id FROM students WHERE roll_number = 'F26-002'), 
        (SELECT id FROM courses WHERE code = 'PHY101'), 
        '2026-01-21', 
        88, 
        'B'
    ),
    (
        (SELECT id FROM students WHERE roll_number = 'F26-003'), 
        (SELECT id FROM courses WHERE code = 'MATH101'), 
        '2026-01-22', 
        95, 
        'A'
    ),
    (
        (SELECT id FROM students WHERE roll_number = 'F26-004'), 
        (SELECT id FROM courses WHERE code = 'PHY101'), 
        '2026-01-22', 
        65, 
        'D'
    );
