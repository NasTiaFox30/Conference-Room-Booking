-- Conference Rooms
CREATE TABLE [dbo].[ConferenceRooms] (
    [RoomId]          INT            IDENTITY (1, 1) NOT NULL,
    [RoomName]        NVARCHAR (100) NOT NULL,
    [Capacity]        INT            NOT NULL,
    [Location]        NVARCHAR (200) NULL,
    [Description]     NVARCHAR (500) NULL,
    [HasProjector]    BIT            DEFAULT ((0)) NULL,
    [HasWhiteboard]   BIT            DEFAULT ((0)) NULL,
    [HasAudioSystem]  BIT            DEFAULT ((0)) NULL,
    [HasWiFi]         BIT            DEFAULT ((0)) NULL,
    [IsActive]        BIT            DEFAULT ((1)) NULL,
    [CreatedDate]     DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([RoomId] ASC)
);

-- Users 
CREATE TABLE [dbo].[Users] (
    [UserId]      INT            IDENTITY (1, 1) NOT NULL,
    [Username]    NVARCHAR (50)  NOT NULL,
    [Email]       NVARCHAR (100) NOT NULL,
    [FirstName]   NVARCHAR (50)  NOT NULL,
    [LastName]    NVARCHAR (50)  NOT NULL,
    [Department]  NVARCHAR (100) NULL,
    [PasswordHash] NVARCHAR (255) NOT NULL,
    [IsAdmin]     BIT            DEFAULT ((0)) NULL,
    [IsActive]    BIT            DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    UNIQUE NONCLUSTERED ([Username] ASC)
);

-- Bookings
CREATE TABLE [dbo].[Bookings] (
    [BookingId]   INT            IDENTITY (1, 1) NOT NULL,
    [RoomId]      INT            NOT NULL,
    [UserId]      INT            NOT NULL,
    [Title]       NVARCHAR (200) NOT NULL,
    [Description] NVARCHAR (500) NULL,
    [StartTime]   DATETIME       NOT NULL,
    [EndTime]     DATETIME       NOT NULL,
    [Attendees]   NVARCHAR (MAX) NULL,
    [Status]      NVARCHAR (20)  DEFAULT ('Confirmed') NULL,
    [CreatedDate] DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([BookingId] ASC),
    CONSTRAINT [FK_Bookings_ConferenceRooms] FOREIGN KEY ([RoomId]) REFERENCES [dbo].[ConferenceRooms] ([RoomId]),
    CONSTRAINT [FK_Bookings_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
);