ALTER TABLE students
ADD COLUMN date_of_birth DATE
CHECK (
    date_of_birth < CURRENT_DATE - INTERVAL '4 years'
    AND date_of_birth > CURRENT_DATE - INTERVAL '24 years'
);