using ConferenceRoomBookingSystem.Data;
using System;
using System.Data.SqlClient;

namespace ConferenceRoomBookingSystem.Tests.TestHelpers
{
    public static class TestDatabaseInitializer
    {
        private static readonly string ConnectionString = new DatabaseHelper().GetConnection().ConnectionString;

        public static void InitializeTestDatabase()
        {
            try
            {
                Console.WriteLine(" == Cleaning and initializing the test database...");

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string sqlScript = @"
                    -- CleanUp
                    DELETE FROM Bookings;
                    DELETE FROM ConferenceRooms;
                    DELETE FROM Users;
                    DBCC CHECKIDENT ('Bookings', RESEED, 0);
                    DBCC CHECKIDENT ('ConferenceRooms', RESEED, 0);
                    DBCC CHECKIDENT ('Users', RESEED, 0);

                    -- Test Conference Rooms
                    INSERT INTO ConferenceRooms (RoomName, Capacity, Location, Description, HasProjector, HasWhiteboard, HasAudioSystem, HasWiFi)
                    VALUES 
                    ('Sala A - Mała sala konferencyjna', 10, '1 piętro, pok. 101', 'Mały pokój do spotkań zespołowych i dyskusji', 1, 1, 0, 1),
                    ('Sala B - Średnia sala konferencyjna', 20, '2 piętro, pok. 205', 'Średnia sala konferencyjna z telewizorem i systemem audio', 1, 1, 1, 1),
                    ('Sala Konferencyjna ""Centralna""', 50, '3 piętro, pok. 301', 'Duża sala konferencyjna do prezentacji i wydarzeń firmowych', 1, 1, 1, 1),
                    ('Sala szkoleniowa', 30, '2 piętro, pok. 210', 'Sala do prowadzenia szkoleń i wydarzeń edukacyjних', 1, 1, 0, 1),
                    ('Przestrzeń kreatywna', 15, '1 piętro, pok. 115', 'Nieformalna przestrzeń do burzy mózgów i sesji kreatywnych', 0, 1, 0, 1);

                    -- Test users for development and testing purposes
                    INSERT INTO Users (Username, Email, FirstName, LastName, Department, PasswordHash, IsAdmin, IsActive)
                    VALUES 
                    ('testuser', 'test@company.com', 'Test', 'User', 'IT', '$2a$11$3TxAntKMlL5rz8IyF3p6de.mAseOdaZnedOA9D213ezJKrf/laBlC', 0, 1),
                    ('adminuser', 'admin@company.com', 'Admin', 'User', 'Management', '$2a$11$3TxAntKMlL5rz8IyF3p6de.mAseOdaZnedOA9D213ezJKrf/laBlC', 1, 1);";

                    using (var command = new SqlCommand(sqlScript, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("✅ Test database successfully initialized!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test DB initialization error: {ex.Message}");
                throw;
            }
        }

        public static void CleanupTestData()
        {
            try
            {
                Console.WriteLine(" == Clearing test data...");

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string cleanupScript = @"
                    DELETE FROM Bookings;
                    DELETE FROM ConferenceRooms WHERE RoomName LIKE '%Test%';
                    DELETE FROM Users WHERE Username LIKE '%testuser_%' OR Username LIKE 'newuser_%';";

                    using (var command = new SqlCommand(cleanupScript, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("✅ Test data successfully cleared!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error clearing test data: {ex.Message}");
            }
        }
    }
}