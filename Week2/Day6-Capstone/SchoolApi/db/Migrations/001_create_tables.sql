CREATE TABLE class_rooms(
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    grade_level INT NOT NULL,
    section VARCHAR(1) NOT NULL,
    academic_year INT NOT NULL
);
CREATE TABLE students(
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    roll_number VARCHAR(50) NOT NULL UNIQUE,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email TEXT NOT NULL UNIQUE,
    class_room_id INT NOT NULL,
    enrolled_on DATE NOT NULL DEFAULT CURRENT_DATE,

    FOREIGN KEY (class_room_id) REFERENCES class_rooms(id)
    ON DELETE RESTRICT
);
CREATE TABLE teachers(
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    employee_code INT UNIQUE NOT NULL,
    full_name VARCHAR(50) NOT NULL,
    email TEXT NOT NULL UNIQUE,
    phone VARCHAR(11) NOT NULL UNIQUE,
    subject VARCHAR(50) NOT NULL,
    hire_date DATE NOT NULL DEFAULT CURRENT_DATE,
    salary numeric(10,2) NOT NULL CHECK(salary>0)
);
CREATE TABLE courses(
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    code VARCHAR(50) NOT NULL,
    title VARCHAR(50) NOT NULL,
    credit_hours INT NOT NULL CHECK(credit_hours>0 and credit_hours<=6),
    capacity INT NOT NULL CHECK(capacity>0 and capacity<=60),
    teacher_id INT NOT NULL,

    FOREIGN KEY (teacher_id) REFERENCES teachers(id)
    ON DELETE RESTRICT
);
CREATE TABLE enrollments(
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    student_id INT NOT NULL,
    course_id INT NOT NULL,
    enrolled_on DATE NOT NULL DEFAULT CURRENT_DATE,
    marks INT CHECK(marks>=0 and marks<=100),
    grade VARCHAR(1) NOT NULL,

    FOREIGN KEY (student_id) REFERENCES students(id)
    ON DELETE RESTRICT,
    FOREIGN KEY (course_id) REFERENCES courses(id)
    ON DELETE RESTRICT,
    CONSTRAINT unique_student_course UNIQUE (student_id, course_id)
);
