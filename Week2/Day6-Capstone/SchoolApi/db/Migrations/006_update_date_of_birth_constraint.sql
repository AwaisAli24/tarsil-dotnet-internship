ALTER TABLE students
DROP CONSTRAINT IF EXISTS students_date_of_birth_check;

ALTER TABLE students
ADD CONSTRAINT students_age_check
CHECK (
    date_of_birth <= CURRENT_DATE - INTERVAL '4 years'
    AND
    date_of_birth > CURRENT_DATE - INTERVAL '26 years'
);