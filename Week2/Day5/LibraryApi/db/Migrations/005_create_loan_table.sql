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