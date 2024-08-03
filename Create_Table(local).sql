
-- Disable all foreign key constraints
DROP TABLE IF EXISTS Users, Presentation, Comment, Faq;

CREATE TABLE forumflow.Users
(
    username VARCHAR(20) NOT NULL UNIQUE,
    passwordSalt VARCHAR(50) NOT NULL,
    passwordHash VARCHAR(100) NOT NULL,
    ID BIGINT NOT NULL AUTO_INCREMENT,
    firstName VARCHAR(20) NOT NULL,
    lastName VARCHAR(20) NOT NULL,
    PRIMARY KEY (ID)
);

CREATE TABLE forumflow.Presentation
(
    authorId BIGINT NOT NULL,
    ID BIGINT NOT NULL AUTO_INCREMENT,
    title VARCHAR(50) NOT NULL,
    description VARCHAR(255) NOT NULL,
    PRIMARY KEY (ID),
    CONSTRAINT FK_Presentation_Users FOREIGN KEY (authorId) REFERENCES forumflow.Users(ID)
        ON DELETE CASCADE
);

CREATE TABLE forumflow.Comment
(
    presentationId BIGINT NOT NULL,
    ID BIGINT NOT NULL AUTO_INCREMENT,
    comment VARCHAR(500) NOT NULL,
    -- 0 for root comment, 1 for first level reply, 2 for second level reply, etc.
    depth INT NOT NULL,
    upCount INT NOT NULL DEFAULT 0,
    downCount INT NOT NULL DEFAULT 0,
    -- null only if root comment, otherwise the ID of the parent comment
    commentParent BIGINT,
    CONSTRAINT FK_Comment_Presentation FOREIGN KEY (presentationId) REFERENCES Presentation(ID)
        ON DELETE CASCADE,
    PRIMARY KEY (ID)
);


CREATE TABLE forumflow.Faq
(
    ID BIGINT NOT NULL AUTO_INCREMENT,
    presentationId BIGINT NOT NULL,
    question VARCHAR(1000) NOT NULL,
    answer VARCHAR(1000) NOT NULL,
    PRIMARY KEY (ID),
    CONSTRAINT FK_Faq_Presentation FOREIGN KEY (presentationId) REFERENCES Presentation(ID)
        ON DELETE CASCADE
);
