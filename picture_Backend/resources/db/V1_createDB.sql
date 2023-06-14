drop database if exists db_picture;
create SCHEMA IF NOT EXISTS db_picture;
use db_picture;

create TABLE IF NOT EXISTS image (
id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
name VARCHAR(45) NOT NULL,
url VARCHAR(45) NOT NULL);