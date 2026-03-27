CREATE TABLE Stack (
    Id          INT             NOT NULL    IDENTITY(1,1)   PRIMARY KEY,
    Name        NVARCHAR(255)   NOT NULL    UNIQUE
);

CREATE TABLE Flashcard (
    Id          INT             NOT NULL    IDENTITY(1,1)   PRIMARY KEY,
    StackId     INT             NOT NULL,
    Question    NVARCHAR(MAX)   NOT NULL,
    Answer      NVARCHAR(MAX)   NOT NULL,

    CONSTRAINT FK_FlashCard_Stack FOREIGN KEY (StackId) REFERENCES Stack(Id) ON DELETE CASCADE
);

CREATE TABLE StudySession (
    Id          INT             NOT NULL    IDENTITY(1,1)   PRIMARY KEY,
    StackId     INT             NOT NULL,
    Date        DATETIME2       NOT NULL,
    Score       INT             NOT NULL,

    CONSTRAINT FK_StudySession_Stack FOREIGN KEY (StackId) REFERENCES Stack(Id) ON DELETE CASCADE
);