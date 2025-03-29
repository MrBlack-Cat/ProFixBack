
IF OBJECT_ID('dbo.ActivityLog', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.ActivityLog;
END


CREATE TABLE dbo.ActivityLog (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Action NVARCHAR(255) NOT NULL,
    EntityType NVARCHAR(255) NOT NULL,
    EntityId INT NOT NULL,

    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME NULL,
    DeletedAt DATETIME NULL,

    CreatedBy INT NULL,
    UpdatedBy INT NULL,
    DeletedBy INT NULL,

    DeletedReason NVARCHAR(2000) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_ActivityLog_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);



ALTER TABLE ClientProfile
ALTER COLUMN AvatarUrl NVARCHAR(1000);
