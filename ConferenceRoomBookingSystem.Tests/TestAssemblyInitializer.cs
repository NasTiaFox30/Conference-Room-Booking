using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConferenceRoomBookingSystem.Tests
{
    [TestClass]
    public class TestAssemblyInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            System.Console.WriteLine(" == Initialization of the test environment...");
            TestHelpers.TestDatabaseInitializer.InitializeTestDatabase();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            System.Console.WriteLine(" == Finish Tests...");
            TestHelpers.TestDatabaseInitializer.CleanupTestData();
        }
    }
}