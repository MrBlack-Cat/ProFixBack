ALTER TABLE Post ADD CreatedBy int null

ALTER TABLE Post ADD IsDeleted BIT NOT NULL DEFAULT 0

ALTER TABLE ServiceProviderProfile ADD IsDeleted BIT DEFAULT 0;
