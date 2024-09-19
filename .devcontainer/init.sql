
CREATE DATABASE testdb;
-- Connect to db
\c testdb;

CREATE TABLE sensor_data (
    sensor_id VARCHAR(100),
    timestamp TIMESTAMP,
    moisture DOUBLE PRECISION,
    sunlight INTEGER,
    temp INTEGER
);
