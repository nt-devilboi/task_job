CREATE TABLE IF NOT EXISTS elements (
    id              BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    attribute_value TEXT NOT NULL,   
    outer_html      TEXT NOT NULL    
);