CREATE TABLE client
(
    id   INTEGER PRIMARY KEY,
    name TEXT
);

CREATE TABLE task
(
    id                 INTEGER PRIMARY KEY,
    status             TEXT,
    description        TEXT,
    estimated_dev_days INTEGER,
    client_id INTEGER,
    FOREIGN KEY (client_id) REFERENCES client (id)
);

CREATE TABLE user
(
    id       INTEGER PRIMARY KEY,
    username TEXT,
    password TEXT,
    client_id INTEGER,
    FOREIGN KEY (client_id) REFERENCES client (id)
);

CREATE TABLE task_assigned
(
    task_id INTEGER,
    user_id INTEGER,
    PRIMARY KEY (task_id, user_id),
    FOREIGN KEY (user_id) REFERENCES user (id),
    FOREIGN KEY (task_id) REFERENCES task (id)
);