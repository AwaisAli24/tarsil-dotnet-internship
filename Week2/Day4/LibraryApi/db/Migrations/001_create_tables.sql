CREATE TABLE authors (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    country VARCHAR(50)
);

CREATE TABLE books (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    isbn VARCHAR(50) UNIQUE NOT NULL,
    price NUMERIC(10, 2) NOT NULL CHECK (price > 0),
    total_copies INT NOT NULL CHECK (total_copies > 0),
    available_copies INT NOT NULL CHECK (
        available_copies >= 0
        AND available_copies <= total_copies
    ),
    published_year INT NOT NULL CHECK (published_year >= 0),
    author_id INT NOT NULL,
    FOREIGN KEY (author_id)
        REFERENCES authors(id)
        ON DELETE RESTRICT
);

CREATE TABLE members (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    full_name VARCHAR(50) NOT NULL,
    email VARCHAR(50) UNIQUE,
    joined_on DATE NOT NULL
);

CREATE TABLE loans (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    book_id INT NOT NULL,
    member_id INT NOT NULL,
    borrowed_at TIMESTAMPTZ NOT NULL,
    due_date TIMESTAMPTZ NOT NULL CHECK (due_date > borrowed_at),
    returned_at TIMESTAMPTZ DEFAULT NULL,
    FOREIGN KEY (book_id)
        REFERENCES books(id)
        ON DELETE RESTRICT,
    FOREIGN KEY (member_id)
        REFERENCES members(id)
        ON DELETE RESTRICT
);