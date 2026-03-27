BEGIN TRAN;

SET NOCOUNT ON;

DECLARE @BaseYear INT = YEAR(SYSUTCDATETIME());

-- 1) Seed stacks (Stack: Id, Name)
INSERT INTO Stack (Name)
SELECT src.Name
FROM (VALUES (N'C#'), (N'SQL'), (N'JavaScript')) AS src(Name)
WHERE NOT EXISTS (
    SELECT 1
    FROM Stack s
    WHERE s.Name = src.Name
);

-- 2) Seed flashcards (Flashcard: Id, StackId, Question, Answer)
INSERT INTO Flashcard (StackId, Question, Answer)
SELECT s.Id, src.Question, src.Answer
FROM (
    VALUES
        (N'C#', N'What keyword defines a class?', N'class'),
        (N'C#', N'What does var do?', N'Implicit local typing'),
        (N'C#', N'How do you declare an async method return type?', N'Task'),
        (N'SQL', N'What clause filters rows?', N'WHERE'),
        (N'SQL', N'What clause groups rows?', N'GROUP BY'),
        (N'SQL', N'How do you sort result rows?', N'ORDER BY'),
        (N'JavaScript', N'How do you declare a constant?', N'const'),
        (N'JavaScript', N'How do you create an arrow function?', N'=>'),
        (N'JavaScript', N'What method parses JSON text?', N'JSON.parse')
) AS src(StackName, Question, Answer)
INNER JOIN Stack s ON s.Name = src.StackName
WHERE NOT EXISTS (
    SELECT 1
    FROM Flashcard f
    WHERE f.StackId = s.Id
      AND f.Question = src.Question
      AND f.Answer = src.Answer
);

-- 3) Seed study sessions for report output (StudySession: Id, StackId, Date, Score)
WITH Numbers AS (
    SELECT 1 AS N
    UNION ALL SELECT 2
    UNION ALL SELECT 3
    UNION ALL SELECT 4
    UNION ALL SELECT 5
),
Seed AS (
    SELECT *
    FROM (
        VALUES
            (N'C#',         0,  1, 3, 3),
            (N'C#',         0,  2, 2, 2),
            (N'C#',         0,  3, 4, 3),
            (N'C#',         0,  5, 3, 3),
            (N'C#',         0,  7, 2, 2),
            (N'C#',         0, 10, 4, 3),
            (N'SQL',        0,  1, 2, 2),
            (N'SQL',        0,  4, 3, 2),
            (N'SQL',        0,  6, 4, 3),
            (N'SQL',        0,  9, 3, 2),
            (N'SQL',        0, 12, 2, 2),
            (N'JavaScript', 0,  2, 3, 1),
            (N'JavaScript', 0,  3, 2, 2),
            (N'JavaScript', 0,  6, 3, 2),
            (N'JavaScript', 0,  8, 4, 3),
            (N'JavaScript', 0, 11, 2, 2),
            (N'C#',        -1,  1, 2, 2),
            (N'C#',        -1,  6, 3, 3),
            (N'C#',        -1, 12, 2, 2),
            (N'SQL',       -1,  2, 2, 2),
            (N'SQL',       -1,  7, 3, 2),
            (N'SQL',       -1, 10, 2, 2),
            (N'JavaScript',-1,  3, 2, 1),
            (N'JavaScript',-1,  8, 3, 2),
            (N'JavaScript',-1, 11, 2, 2)
    ) AS t(StackName, YearOffset, MonthNumber, SessionCount, BaseScore)
),
Expanded AS (
    SELECT
        s.Id AS StackId,
        DATEFROMPARTS(@BaseYear + seed.YearOffset, seed.MonthNumber, 2 + (num.N * 4)) AS SessionDate,
        CASE
            WHEN seed.BaseScore + (((num.N + seed.MonthNumber) % 3) - 1) < 0 THEN 0
            WHEN seed.BaseScore + (((num.N + seed.MonthNumber) % 3) - 1) > 3 THEN 3
            ELSE seed.BaseScore + (((num.N + seed.MonthNumber) % 3) - 1)
        END AS SessionScore
    FROM Seed seed
    INNER JOIN Stack s ON s.Name = seed.StackName
    INNER JOIN Numbers num ON num.N <= seed.SessionCount
)
INSERT INTO StudySession (StackId, Date, Score)
SELECT exp.StackId, exp.SessionDate, exp.SessionScore
FROM Expanded exp
WHERE NOT EXISTS (
    SELECT 1
    FROM StudySession ss
    WHERE ss.StackId = exp.StackId
      AND ss.Date = exp.SessionDate
      AND ss.Score = exp.SessionScore
);

ROLLBACK;