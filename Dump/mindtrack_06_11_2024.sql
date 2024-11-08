-- DROP DATABASE mindtrack;
CREATE DATABASE IF NOT EXISTS mindtrack;
USE mindtrack;

DROP TABLE IF EXISTS score;
DROP TABLE IF EXISTS person;
DROP TABLE IF EXISTS account;
DROP TABLE IF EXISTS game;

CREATE TABLE IF NOT EXISTS account (
	id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(100) NOT NULL,
    password VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS person (
    id INT AUTO_INCREMENT PRIMARY KEY,
    account_id INT,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100),
    birthday DATE,
    FOREIGN KEY (account_id) REFERENCES account(id) ON DELETE CASCADE
);

CREATE TABLE game (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS score (
    id INT AUTO_INCREMENT PRIMARY KEY,
    person_id INT,
    game_id INT,
    game_score INT NOT NULL,
    date_played DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (person_id) REFERENCES person(id) ON DELETE CASCADE,
    FOREIGN KEY (game_id) REFERENCES game(id) ON DELETE CASCADE
);

INSERT INTO account (username, password) VALUES
('pnaucke', 'welkom01'),
('jmhol', 'welkom02'),
('pbasri', 'welkom03'),
('wboulayoune', 'welkom04'),
('jvandertol', 'welkom05');

INSERT INTO person (account_id, name, email, birthday) VALUES
(1, 'Pieter Naucke', 'p.naucke@student.fontys.nl', '1944-05-12'),
(2, 'Juan Manuel Hol', 'jm.hol@student.fontys.nl', '1936-07-23'),
(3, 'Puteri Basri', 'p.basri@student.fontys.nl', '1919-11-15'),
(4, 'Wassila Boulayoune', 'w.boulayoune@student.fontys.nl', '1924-08-21'),
(5, 'Jesper van der Tol', 'j.vandertol@student.fontys.nl', '1930-09-30');

INSERT INTO game (name) VALUES
('Memory'),
('ReactionSpeed');

INSERT INTO score (person_id, game_id, game_score, date_played) VALUES
(1, 1, 28, '2024-11-03 14:20:00'), -- Pieter Naucke plays Memory
(1, 2, 220, '2024-11-04 16:10:00'), -- Pieter Naucke plays ReactionSpeed
(2, 1, 19, '2024-11-03 11:30:00'), -- Juan Manuel Hol plays Memory
(2, 2, 180, '2024-11-03 12:45:00'), -- Juan Manuel Hol plays ReactionSpeed
(3, 1, 24, '2024-11-04 09:50:00'), -- Puteri Basri plays Memory
(3, 2, 270, '2024-11-05 13:15:00'), -- Puteri Basri plays ReactionSpeed
(4, 1, 30, '2024-11-03 10:00:00'), -- Wassila Boulayoune plays Memory
(4, 2, 190, '2024-11-03 11:30:00'), -- Wassila Boulayoune plays ReactionSpeed
(5, 1, 22, '2024-11-04 15:00:00'), -- Jesper van der Tol plays Memory
(5, 2, 210, '2024-11-05 17:20:00'), -- Jesper van der Tol plays ReactionSpeed
(1, 1, 25, '2024-11-05 14:35:00'), -- Pieter Naucke plays Memory again
(2, 2, 185, '2024-11-05 16:40:00'), -- Juan Manuel Hol plays ReactionSpeed again
(4, 1, 29, '2024-11-06 09:30:00'), -- Wassila Boulayoune plays Memory again
(5, 2, 215, '2024-11-06 12:15:00'); -- Jesper van der Tol plays ReactionSpeed again

