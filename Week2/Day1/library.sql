DROP TABLE IF EXISTS authors,
books,
members,
loans;

CREATE TABLE authors(
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    country VARCHAR(50)
);

CREATE TABLE books(
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    title VARCHAR(50) NOT NULL,
    isbn VARCHAR(50) UNIQUE NOT NULL,
    price NUMERIC(10, 2) NOT NULL CHECK(price > 0),
    total_copies INT NOT NULL,
    available_copies INT NOT NULL CHECK(
        0 <= available_copies
        AND available_copies <= total_copies
    ),
    published_year DATE NOT NULL,
    author_id int NOT NULL,
    FOREIGN KEY (author_id) REFERENCES authors(id) ON DELETE RESTRICT
);

CREATE TABLE members(
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    full_name VARCHAR(50) NOT NULL,
    email VARCHAR(50) UNIQUE,
    joined_on DATE NOT NULL
);

CREATE TABLE loans(
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    book_id INT NOT NULL,
    member_id INT NOT NULL,
    borrowed_at TIMESTAMPTZ NOT NULL,
    due_date TIMESTAMPTZ NOT NULL CHECK(due_date > borrowed_at),
    returned_at TIMESTAMPTZ DEFAULT NULL,
    FOREIGN KEY (book_id) REFERENCES books(id) ON DELETE RESTRICT,
    FOREIGN KEY (member_id) REFERENCES members(id) ON DELETE RESTRICT
);

INSERT INTO
    authors (name, country)
VALUES
    ('Fyodor Dostoevsky', 'Russia'),
    ('Franz Kafka', 'Austria-Hungary'),
    ('Albert Camus', 'France');

INSERT INTO
    books (
        title,
        isbn,
        price,
        total_copies,
        available_copies,
        published_year,
        author_id
    )
VALUES
    (
        'Crime and Punishment',
        '9780140449136',
        1200.00,
        5,
        3,
        '1866-01-01',
        1
    ),
    (
        'The Brothers Karamazov',
        '9780374528379',
        1500.00,
        4,
        3,
        '1880-01-01',
        1
    ),
    (
        'The Idiot',
        '9780140447927',
        1100.00,
        3,
        2,
        '1869-01-01',
        1
    ),
    (
        'The Metamorphosis',
        '9780553213690',
        800.00,
        6,
        4,
        '1915-01-01',
        2
    ),
    (
        'The Trial',
        '9780805209990',
        950.00,
        4,
        3,
        '1925-01-01',
        2
    ),
    (
        'The Stranger',
        '9780679720201',
        900.00,
        5,
        3,
        '1942-01-01',
        3
    ),
    (
        'The Plague',
        '9780679720218',
        1300.00,
        4,
        3,
        '1947-01-01',
        3
    ),
    (
        'The Fall',
        '9780679720225',
        850.00,
        3,
        2,
        '1956-01-01',
        3
    );

INSERT INTO
    members (full_name, email, joined_on)
VALUES
    ('Ali Ahmed', 'ali@example.com', '2025-01-10'),
    ('Sara Khan', 'sara@example.com', '2025-02-15'),
    ('Usman Malik', 'usman@example.com', '2025-03-20'),
    (
        'Ayesha Noor',
        'ayesha@example.com',
        '2025-04-05'
    ),
    ('Hamza Tariq', 'hamza@example.com', '2025-05-12');

INSERT INTO
    loans (
        book_id,
        member_id,
        borrowed_at,
        due_date,
        returned_at
    )
VALUES
    (
        1,
        1,
        NOW() - INTERVAL '30 days',
        NOW() - INTERVAL '16 days',
        NOW() - INTERVAL '20 days'
    ),
    (
        4,
        2,
        NOW() - INTERVAL '25 days',
        NOW() - INTERVAL '11 days',
        NOW() - INTERVAL '12 days'
    ),
    (
        2,
        3,
        NOW() - INTERVAL '3 days',
        NOW() + INTERVAL '11 days',
        NULL
    ),
    (
        5,
        1,
        NOW() - INTERVAL '25 days',
        NOW() - INTERVAL '11 days',
        NULL
    ),
    (
        6,
        4,
        NOW() - INTERVAL '20 days',
        NOW() - INTERVAL '6 days',
        NULL
    ),
    (
        3,
        5,
        NOW() - INTERVAL '2 days',
        NOW() + INTERVAL '12 days',
        NULL
    );