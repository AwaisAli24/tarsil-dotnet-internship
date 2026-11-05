INSERT INTO authors (name, country)
VALUES
    ('Fyodor Dostoevsky', 'Russia'),
    ('Franz Kafka', 'Austria-Hungary'),
    ('Albert Camus', 'France')
    ON CONFLICT DO NOTHING;

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
    ('Crime and Punishment', '9780140449136', 1200.00, 5, 5, 1866, 1),
    ('The Brothers Karamazov', '9780374528379', 1500.00, 4, 4, 1880, 1),
    ('The Idiot', '9780140447927', 1100.00, 3, 3, 1869, 1),
    ('The Metamorphosis', '9780553213690', 800.00, 6, 6, 1915, 2),
    ('The Trial', '9780805209990', 950.00, 4, 4, 1925, 2),
    ('The Stranger', '9780679720201', 900.00, 5, 5, 1942, 3),
    ('The Plague', '9780679720218', 1300.00, 4, 4, 1947, 3),
    ('The Fall', '9780679720225', 850.00, 3, 3, 1956, 3)
    ON CONFLICT DO NOTHING;

INSERT INTO members (full_name, email, joined_on)
VALUES
    ('Ali Ahmed', 'ali@example.com', '2025-01-10'),
    ('Sara Khan', 'sara@example.com', '2025-02-15'),
    ('Usman Malik', 'usman@example.com', '2025-03-20'),
    ('Ayesha Noor', 'ayesha@example.com', '2025-04-05'),
    ('Hamza Tariq', 'hamza@example.com', '2025-05-12')
    ON CONFLICT DO NOTHING;