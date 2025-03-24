-- 1. Yeni 'Name' ve 'Surname' sütunlarını ekle
ALTER TABLE ServiceProviderProfile ADD [Name] NVARCHAR(100) NOT NULL ,
                           Surname NVARCHAR(100) NOT NULL ;

-- 2. FullName'deki veriyi parçalayarak Name ve Surname sütunlarına aktar
UPDATE ServiceProviderProfile
SET Name = LEFT(FullName, CHARINDEX(' ', FullName + ' ') - 1), 
    Surname = SUBSTRING(FullName, CHARINDEX(' ', FullName + ' ') + 1, LEN(FullName));

-- 3. FullName sütununu kaldır
ALTER TABLE ServiceProviderProfile DROP COLUMN FullName;