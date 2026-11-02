INSERT INTO authors (name, country)
VALUES
    ('Fyodor Dostoevsky', 'Russia'),
    ('Franz Kafka', 'Austria-Hungary'),
    ('Albert Camus', 'France');

INSERT INTO books (
    title,
    isbn,
    price,
    total_copies,
    available_copies,
    published_year,
    author_id
)
VALUES
    ('Crime and Punishment', '9780140449136', 1200.00, 5, 3, 1866, 1),
    ('The Brothers Karamazov', '9780374528379', 1500.00, 4, 3, 1880, 1),
    ('The Idiot', '9780140447927', 1100.00, 3, 2, 1869, 1),
    ('The Metamorphosis', '9780553213690', 800.00, 6, 5, 1915, 2),
    ('The Trial', '9780805209990', 950.00, 4, 3, 1925, 2),
    ('The Stranger', '9780679720201', 900.00, 5, 4, 1942, 3),
    ('The Plague', '9780679720218', 1300.00, 4, 4, 1947, 3),
    ('The Fall', '9780679720225', 850.00, 3, 3, 1956, 3);

INSERT INTO members (full_name, email, joined_on)
VALUES
    ('Ali Ahmed', 'ali@example.com', '2025-01-10'),
    ('Sara Khan', 'sara@example.com', '2025-02-15'),
    ('Usman Malik', 'usman@example.com', '2025-03-20'),
    ('Ayesha Noor', 'ayesha@example.com', '2025-04-05'),
    ('Hamza Tariq', 'hamza@example.com', '2025-05-12');

INSERT INTO loans (
    book_id,
    member_id,
    borrowed_at,
    due_date,
    returned_at
)
VALUES
    (
        1, 1,
        NOW() - INTERVAL '3 days',
        NOW() + INTERVAL '11 days',
        NULL
    ),
    (
        1, 2,
        NOW() - INTERVAL '2 days',
        NOW() + INTERVAL '12 days',
        NULL
    ),
    (
        2, 3,
        NOW() - INTERVAL '3 days',
        NOW() + INTERVAL '11 days',
        NULL
    ),
    (
        3, 5,
        NOW() - INTERVAL '2 days',
        NOW() + INTERVAL '12 days',
        NULL
    ),
    (
        4, 2,
        NOW() - INTERVAL '25 days',
        NOW() - INTERVAL '11 days',
        NOW() - INTERVAL '12 days'
    ),
    (
        5, 1,
        NOW() - INTERVAL '25 days',
        NOW() - INTERVAL '11 days',
        NULL
    ),
    (
        6, 4,
        NOW() - INTERVAL '2 days',
        NOW() + INTERVAL '12 days',
        NULL
    );