INSERT INTO [QuizMaster].[dbo].[Categories] ([Name])
SELECT v.[Name]
FROM (VALUES
    (N'Jêzyki obce'),
    (N'Programowanie'),
    (N'Matematyka'),
    (N'Historia'),
    (N'Geografia'),
    (N'Biologia'),
    (N'Chemia'),
    (N'Fizyka'),
    (N'Informatyka'),
    (N'Bazy danych'),
    (N'Sieci komputerowe'),
    (N'Bezpieczeñstwo IT'),
    (N'Algorytmy'),
    (N'Architektura oprogramowania'),
    (N'Ekonomia'),
    (N'Prawo'),
    (N'Medycyna'),
    (N'Psychologia'),
    (N'Kultura i sztuka'),
    (N'Inne')
) AS v([Name])
WHERE NOT EXISTS (
    SELECT 1
    FROM [QuizMaster].[dbo].[Categories] c
    WHERE c.[Name] = v.[Name]
);