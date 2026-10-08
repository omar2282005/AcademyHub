/*
AcademyHub — SQL Server demo data

1. Apply TrainingDbContext migrations first.
2. Select AcademyHubTrainingDb in SSMS before running the whole file.
3. Run on a DEVELOPMENT database; all names below are fictional.
4. No DELETE, UPDATE, TRUNCATE, or Identity-account insertion is performed.
5. Re-running sequentially reuses these demo records; existing values are not overwritten.

Default demo course owner: demo-seed (a marker, NOT an Identity account).
Public catalog/details work, but a registered user cannot edit these courses.
For ownership testing, create your own course through the application.
Alternatively, replace @DemoOwnerUserId with your already registered account Id
before the FIRST run. The app uses a separate Identity database, so this script
cannot validate that account. It never reassigns existing demo course owners.

If you intentionally renamed the training database, change @ExpectedDatabase.
Review actual migrated schema before use; this file was not executed against
SQL Server in the assistant environment.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @ExpectedDatabase sysname = N'AcademyHubTrainingDb';
DECLARE @DemoOwnerUserId nvarchar(450) = N'demo-seed';

IF DB_NAME() <> @ExpectedDatabase
    THROW 50001, 'Select the training database before running seed-demo.sql.', 1;

IF NULLIF(LTRIM(RTRIM(@DemoOwnerUserId)), N'') IS NULL
    THROW 50002, 'The demo course owner marker must not be empty.', 1;

IF OBJECT_ID(N'dbo.Instructors', N'U') IS NULL
   OR OBJECT_ID(N'dbo.InstructorProfiles', N'U') IS NULL
   OR OBJECT_ID(N'dbo.Students', N'U') IS NULL
   OR OBJECT_ID(N'dbo.Courses', N'U') IS NULL
   OR OBJECT_ID(N'dbo.Enrollments', N'U') IS NULL
    THROW 50003, 'Apply TrainingDbContext migrations before running the seed.', 1;

DECLARE @DemoInstructors TABLE
(
    Id nvarchar(450) PRIMARY KEY,
    Name nvarchar(100) NOT NULL,
    Bio nvarchar(1000) NOT NULL
);

INSERT INTO @DemoInstructors (Id, Name, Bio)
VALUES
    (N'demo-instructor-ahmed', N'Demo: Ahmed Hassan',
     N'Fictional instructor profile for testing ASP.NET Core MVC and Entity Framework Core course screens.'),
    (N'demo-instructor-mariam', N'Demo: Mariam Adel',
     N'Fictional instructor profile for testing SQL Server and relational database course screens.');

DECLARE @DemoStudents TABLE
(
    Name nvarchar(100) PRIMARY KEY
);

INSERT INTO @DemoStudents (Name)
VALUES
    (N'Demo: Omar Khaled'),
    (N'Demo: Sara Mohamed'),
    (N'Demo: Youssef Ali'),
    (N'Demo: Nour Ahmed'),
    (N'Demo: Salma Tarek');

DECLARE @DemoCourses TABLE
(
    Title nvarchar(100) NOT NULL,
    InstructorId nvarchar(450) NOT NULL,
    PRIMARY KEY (Title, InstructorId)
);

INSERT INTO @DemoCourses (Title, InstructorId)
VALUES
    (N'Demo: ASP.NET Core MVC', N'demo-instructor-ahmed'),
    (N'Demo: Entity Framework Core', N'demo-instructor-ahmed'),
    (N'Demo: SQL Server Fundamentals', N'demo-instructor-mariam');

DECLARE @DemoEnrollments TABLE
(
    StudentName nvarchar(100) NOT NULL,
    CourseTitle nvarchar(100) NOT NULL,
    InstructorId nvarchar(450) NOT NULL,
    EnrolledAt datetime2 NOT NULL,
    Grade float NULL
);

/* Dates are fixed demo values interpreted as UTC by this seed's convention. */
INSERT INTO @DemoEnrollments
    (StudentName, CourseTitle, InstructorId, EnrolledAt, Grade)
VALUES
    (N'Demo: Omar Khaled', N'Demo: ASP.NET Core MVC', N'demo-instructor-ahmed', '2026-09-01T10:00:00', 92),
    (N'Demo: Sara Mohamed', N'Demo: ASP.NET Core MVC', N'demo-instructor-ahmed', '2026-09-03T12:30:00', 87),
    (N'Demo: Youssef Ali', N'Demo: ASP.NET Core MVC', N'demo-instructor-ahmed', '2026-09-06T09:15:00', NULL),
    (N'Demo: Nour Ahmed', N'Demo: Entity Framework Core', N'demo-instructor-ahmed', '2026-09-08T14:00:00', 78),
    (N'Demo: Salma Tarek', N'Demo: Entity Framework Core', N'demo-instructor-ahmed', '2026-09-10T11:45:00', 55);

/* SQL Server Fundamentals intentionally has no enrollments: empty-state demo. */

BEGIN TRY
    BEGIN TRANSACTION;

    /* Do not silently treat a real record using a reserved Id as a demo record. */
    IF EXISTS
    (
        SELECT 1
        FROM dbo.Instructors AS i
        INNER JOIN @DemoInstructors AS d ON d.Id = i.Id
        WHERE i.Name <> d.Name
    )
        THROW 50004, 'A reserved demo instructor Id already belongs to a differently named record.', 1;

    /* Names are not unique in the model. Stop if a demo name is ambiguous. */
    IF EXISTS
    (
        SELECT s.Name
        FROM dbo.Students AS s
        INNER JOIN @DemoStudents AS d ON d.Name = s.Name
        GROUP BY s.Name
        HAVING COUNT(*) > 1
    )
        THROW 50005, 'Duplicate demo student names exist; resolve them before seeding.', 1;

    IF EXISTS
    (
        SELECT c.Title, c.InstructorId
        FROM dbo.Courses AS c
        INNER JOIN @DemoCourses AS d
            ON d.Title = c.Title AND d.InstructorId = c.InstructorId
        GROUP BY c.Title, c.InstructorId
        HAVING COUNT(*) > 1
    )
        THROW 50006, 'Duplicate demo course titles for the same instructor exist; resolve them before seeding.', 1;

    INSERT INTO dbo.Instructors (Id, Name)
    SELECT d.Id, d.Name
    FROM @DemoInstructors AS d
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.Instructors AS i WHERE i.Id = d.Id
    );

    INSERT INTO dbo.InstructorProfiles (Bio, InstructorId)
    SELECT d.Bio, d.Id
    FROM @DemoInstructors AS d
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.InstructorProfiles AS p WHERE p.InstructorId = d.Id
    );

    INSERT INTO dbo.Students (Name)
    SELECT d.Name
    FROM @DemoStudents AS d
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.Students AS s WHERE s.Name = d.Name
    );

    INSERT INTO dbo.Courses (Title, InstructorId, CreatedByUserId)
    SELECT d.Title, d.InstructorId, @DemoOwnerUserId
    FROM @DemoCourses AS d
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.Courses AS c
        WHERE c.Title = d.Title AND c.InstructorId = d.InstructorId
    );

    INSERT INTO dbo.Enrollments (StudentId, CourseId, EnrolledAt, Grade)
    SELECT s.Id, c.Id, d.EnrolledAt, d.Grade
    FROM @DemoEnrollments AS d
    INNER JOIN dbo.Students AS s ON s.Name = d.StudentName
    INNER JOIN dbo.Courses AS c
        ON c.Title = d.CourseTitle AND c.InstructorId = d.InstructorId
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.Enrollments AS e
        WHERE e.StudentId = s.Id AND e.CourseId = c.Id
    );

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;

/* Show actual generated Ids; do not assume identity values start at 1. */
SELECT
    c.Id AS CourseId,
    c.Title,
    i.Name AS InstructorName,
    c.CreatedByUserId,
    COUNT(e.StudentId) AS StudentCount
FROM dbo.Courses AS c
INNER JOIN @DemoCourses AS d
    ON d.Title = c.Title AND d.InstructorId = c.InstructorId
INNER JOIN dbo.Instructors AS i ON i.Id = c.InstructorId
LEFT JOIN dbo.Enrollments AS e ON e.CourseId = c.Id
GROUP BY c.Id, c.Title, i.Name, c.CreatedByUserId
ORDER BY c.Id;

SELECT
    c.Title AS CourseTitle,
    s.Name AS StudentName,
    e.EnrolledAt,
    e.Grade
FROM dbo.Enrollments AS e
INNER JOIN dbo.Courses AS c ON c.Id = e.CourseId
INNER JOIN @DemoCourses AS d
    ON d.Title = c.Title AND d.InstructorId = c.InstructorId
INNER JOIN dbo.Students AS s ON s.Id = e.StudentId
ORDER BY c.Id, s.Id;
