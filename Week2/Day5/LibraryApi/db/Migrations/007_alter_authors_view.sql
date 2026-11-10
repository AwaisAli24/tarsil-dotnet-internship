CREATE OR REPLACE VIEW authors_view AS
SELECT a.name, count(b.id) as total_books,coalesce( SUM( b.total_copies),0) as total_copies,coalesce (SUM(b.total_copies-b.available_copies),0) as currently_borrowed
FROM authors a
LEFT JOIN books b
ON b.author_id = a.id
GROUP BY a.name;