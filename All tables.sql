CREATE TABLE [dbo].[About_Us] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (500) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Image]       VARCHAR (MAX)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Advertisement] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (500) NULL,
    [Image]          VARCHAR (MAX)  NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [Web_Address]    NVARCHAR (200) NULL,
    [Social_Address] NVARCHAR (100) NULL,
    [Phone_Num]      NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Teacher] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [First_Name]  NVARCHAR (50)  NULL,
    [Second_Name] NVARCHAR (50)  NULL,
    [Password]    NVARCHAR (50)  NULL,
    [Address]     NVARCHAR (500) NULL,
    [Gender]      NVARCHAR (20)  NULL,
    [User_Name]   NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);




CREATE TABLE [dbo].[Course] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (500) NULL,
    [Image]       VARCHAR (MAX)  NULL,
    [Start_Date]  DATETIME       NULL,
    [End_Date]    DATETIME       NULL,
    [Views]       INT            DEFAULT ((0)) NOT NULL,
    [Likes]       INT            DEFAULT ((0)) NOT NULL,
    [Address]     NVARCHAR (500) NULL,
    [Course_Type] NVARCHAR (50)  NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Duration]    INT            DEFAULT ((0)) NOT NULL,
    [Created_At]  DATETIME       NULL,
    [Updated_At]  DATETIME       NULL,
    [Teacher_Id]  INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Course_ToTeacher] FOREIGN KEY ([Teacher_Id]) REFERENCES [dbo].[Teacher] ([Id])
);

CREATE TABLE [dbo].[Favourite_Courses] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [User_Id]   INT NULL,
    [Course_Id] INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Favourite_Courses_ToUser] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Favourite_Courses_ToCourses] FOREIGN KEY ([Course_Id]) REFERENCES [dbo].[Course] ([Id])
);

CREATE TABLE [dbo].[Like_Courses] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [User_Id]   INT NULL,
    [Course_Id] INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Like_Courses_ToUser] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Like_Courses_ToCourses] FOREIGN KEY ([Course_Id]) REFERENCES [dbo].[Course] ([Id])
);

CREATE TABLE [dbo].[Subscription_Courses] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [User_Id]   INT NULL,
    [Course_Id] INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Subscription_Courses_ToUser] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Subscription_Courses_ToCourses] FOREIGN KEY ([Course_Id]) REFERENCES [dbo].[Course] ([Id])
);

CREATE TABLE [dbo].[User] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [First_Name]  NVARCHAR (50)  NULL,
    [Second_Name] NVARCHAR (50)  NULL,
    [Password]    NVARCHAR (50)  NULL,
    [Address]     NVARCHAR (500) NULL,
    [Gender]      NVARCHAR (20)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



