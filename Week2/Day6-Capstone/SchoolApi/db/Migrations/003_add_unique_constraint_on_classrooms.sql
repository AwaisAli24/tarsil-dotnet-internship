ALTER TABLE class_rooms
DROP CONSTRAINT class_rooms_name_key;

ALTER TABLE class_rooms
ADD CONSTRAINT unique_classroom UNIQUE (name, academic_year);
