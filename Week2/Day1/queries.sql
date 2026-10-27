-- 1.
SELECT b.*, a.name
FROM books b
JOIN authors a ON b.author_id = a.id;
-- 2.
SELECT * FROM books
WHERE price >= 500 and price <= 1500
ORDER BY price asc;
--3.
SELECT * FROM books
where title ILIKE '%the%';
--4.
SELECT * FROM books
OFFSET 3 LIMIT 3
--5.
SELECT a.id,a.name,count(b.id) FROM authors a
LEFT JOIN books b
ON a.id= b.author_id
GROUP BY a.id
--6.
SELECT * FROM members
WHERE id IN (
	SELECT member_id FROM loans WHERE returned_at IS NULL
)
--7.
SELECT b.title,m.full_name FROM loans l
LEFT JOIN books b ON l.book_id = b.id
LEFT JOIN members m ON l.member_id = m.id
WHERE returned_at is NULL or due_date < returned_at
--8.
SELECT title FROM books
WHERE id IN (SELECT book_id FROM loans GROUP BY book_id ORDER BY count(book_id))
LIMIT 3
--9.
SELECT title FROM books
WHERE id IN (SELECT book_id FROM loans GROUP BY book_id ORDER BY count(book_id) desc)
LIMIT 3
-- 10.
SELECT name FROM authors
Where id in (SELECT author_id from books GROUP BY author_id having avg(price)>1000)
-- 11.
INSERT INTO books (title,isbn,price,total_copies,available_copies,published_year,author_id)
VALUES
    (
        'White Nights',
        '9780140449189',
        800.00,
        5,
        3,
        '1866-01-01',
        1
    )
	returning id;
-- 12.
-- ERROR:  Key (id)=(1) is referenced from table "books".update or delete on table "authors" violates RESTRICT setting of foreign key constraint "books_author_id_fkey" on table "books" 

-- ERROR:  update or delete on table "authors" violates RESTRICT setting of foreign key constraint "books_author_id_fkey" on table "books"
-- SQL state: 23001
-- Detail: Key (id)=(1) is referenced from table "books".
-- The Error says that you cannot delete or update the author with id=1 because there are books that reference this author in the "books" table. The foreign key constraint "books_author_id_fkey" is set to RESTRICT, which means that you cannot delete or update a record in the "authors" table if it is being referenced by any record in the "books" table. To resolve this, you would need to either delete the books that reference this author first or change the foreign key constraint to allow cascading deletes or updates.