CREATE VIEW authors_view AS
SELECT a.name, count(b.id) as total_books,SUM( b.total_copies) as total_copies,SUM(b.total_copies-b.available_copies) as currently_borrowed
FROM books b
JOIN authors a
ON b.author_id = a.id
GROUP BY a.name;