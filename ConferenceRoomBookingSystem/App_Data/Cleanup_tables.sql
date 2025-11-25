-- 1. Clean parent table first to avoid foreign key constraint issues
TRUNCATE TABLE [dbo].[Bookings];

-- 2. Clean child tables to remove dependencies
TRUNCATE TABLE [dbo].[ConferenceRooms];
TRUNCATE TABLE [dbo].[Users];