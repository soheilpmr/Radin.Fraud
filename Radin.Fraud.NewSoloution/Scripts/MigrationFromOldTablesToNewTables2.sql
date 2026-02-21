-- This should return the top 5 users from your remote database
--SELECT * FROM [10.81.15.128].[FraudPSP].[dbo].[USER];

USE [FraudAuthDB]
GO

-- Assuming the remote server is configured as a Linked Server named '10.81.15.128'
-- If using a different Linked Server name, update the FROM clause accordingly.

INSERT INTO [dbo].[AspNetUsers] (
    [Id], 
    [AllowedIPs], 
    [FirstName], 
    [LastName], 
    [Email], 
    [PhoneNumber], 
    [IsEnabled], 
    [LastLogin], 
    [UserName], 
    [NormalizedUserName], 
    [NormalizedEmail], 
    [EmailConfirmed], 
    [PasswordHash], 
    [SecurityStamp], 
    [ConcurrencyStamp], 
    [PhoneNumberConfirmed], 
    [TwoFactorEnabled], 
    [LockoutEnabled], 
    [AccessFailedCount]
)
SELECT 
    -- ASP.NET Identity defaults to GUIDs as strings for the Id. 
    -- We cast your bigint ID to nvarchar to preserve relational integrity.
    CAST([ID] AS nvarchar(450)), 
    
    ISNULL([ALLOWEDIPS], ''), 
    [FIRSTNAME], 
    [LASTNAME], 
    ISNULL([EMAIL], ''), 
    ISNULL([PHONENUMBER], ''), 
    [ISENABLED], 
    [LASTLOGIN], 
    [USERNAME], 
    
    -- Identity requires Normalized fields (usually uppercase) for fast lookups
    UPPER([USERNAME]), 
    UPPER([EMAIL]), 
    
    -- Defaulting confirmations and Identity-specific fields
    0, -- EmailConfirmed (Set to 1 if you want to consider legacy emails pre-confirmed)
    [PASSWORD], 
    
    -- Stamps are mandatory for ASP.NET Identity session validation
    CAST(NEWID() AS nvarchar(max)), -- SecurityStamp
    CAST(NEWID() AS nvarchar(max)), -- ConcurrencyStamp
    
    0, -- PhoneNumberConfirmed
    0, -- TwoFactorEnabled
    1, -- LockoutEnabled (Usually 1/true by default to allow lockout on failed attempts)
    0  -- AccessFailedCount
FROM 
    [10.81.15.128].[FraudPSP].[dbo].[USER];
GO

-----------------------------------------------------USER Table Migration Ended----------------------------------------------------------

USE [FraudAuthDB]
GO

INSERT INTO [dbo].[AspNetRoles] (
    [Id],
    [IsEnabled],
    [Name],
    [NormalizedName],
    [ConcurrencyStamp]
)
SELECT 
    -- Cast the bigint ID to nvarchar to match Identity's format 
    -- and preserve the exact IDs for our next step
    CAST([ID] AS nvarchar(450)), 
    
    [ISENABLED], 
    
    -- Cast to 256 to safely match the destination schema
    CAST([NAME] AS nvarchar(256)), 
    
    -- Identity requires an uppercase NormalizedName for lookups
    CAST(UPPER([NAME]) AS nvarchar(256)), 
    
    -- Required for Identity concurrency tracking
    CAST(NEWID() AS nvarchar(max)) 
FROM 
    [10.81.15.128].[FraudPSP].[dbo].[ROLE];
GO


-----------------------------------------------------AspNetRoles Table Migration Ended----------------------------------------------------------


USE [FraudAuthDB]
GO

INSERT INTO [dbo].[AspNetUserRoles] (
    [UserId],
    [RoleId]
)
SELECT 
    CAST([ID] AS nvarchar(450)), 
    CAST([ROLEID] AS nvarchar(450))
FROM 
    [10.81.15.128].[FraudPSP].[dbo].[USER]
WHERE 
    [ROLEID] IS NOT NULL;
GO

-----------------------------------------------------AspNetUserRoles Table Migration Ended----------------------------------------------------------
