using data_access_layer.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Linq;
using Model = data_access_layer.Model.Db;

namespace crawler_test
{
    public sealed class TestDbContext : PrinterContext
    {
        public TestDbContext(DbContextOptions<PrinterContext> options) : base(options)
        {
            Database.OpenConnection();
            Database.EnsureCreated();
        }
        public override void Dispose()
        {
            Database.CloseConnection();
            base.Dispose();
        }
    }
    [TestFixture]
    class PrinterDbTests
    {
        // Options for test context in memory.
        readonly DbContextOptions<PrinterContext> options = new DbContextOptionsBuilder<PrinterContext>()
                .UseSqlite("DataSource=file::memory:")
                // For navigational properties usage.
                .UseLazyLoadingProxies()
                .Options;

        [TestCase("TestCity", "TestOrg", "TestRoom", "TestType", 0, 0, 0, "TestRoomHistory")]
        public void DbDataSeed_IsDataInsertedAndEqual_True(
            string cityName, string orgName, string roomName, string typeName, 
            int tonerLevel, int drumLevel, int totalPages, string historyRoomName)
        {
            // Feeding with test data.
            using (var context = new TestDbContext(options))
            {
                Model.City city = new Model.City()
                {
                    Name = cityName,
                    Organizations = new Collection<Model.Organization>()
                    {
                        new Model.Organization()
                        {
                            Name = orgName,
                            Rooms = new Collection<Model.Room>()
                            {
                                new Model.Room()
                                {
                                    Name = roomName,
                                    Printers = new Collection<Model.Printer>()
                                    {
                                        new Model.Printer()
                                        {
                                            Statistic = new Model.Statistic()
                                            {
                                                TonerLevel = tonerLevel,
                                                DrumLevel = drumLevel,
                                                TotalPagesPrinted = totalPages
                                            },
                                            Type = new Model.Type()
                                            {
                                                Name = typeName
                                            }
                                        }
                                    },
                                    RoomHistory = new Model.RoomHistory()
                                    {
                                        Name = historyRoomName
                                    }
                                }
                            }
                        }
                    }
                };

                context.Cities.Add(city);
                context.SaveChanges();

                // Make sure if test data was correctly saved.
                var cityDb = context.Cities.Find(1);
                var org = cityDb.Organizations.Where(e => e.Id == 1).FirstOrDefault();
                var room = org.Rooms.Where(e => e.Id == 1).FirstOrDefault();

                Assert.AreEqual(cityName, cityDb.Name);
                Assert.AreEqual(orgName, org.Name);
                Assert.AreEqual(roomName, room.Name);
                Assert.AreEqual(typeName, room.Printers.FirstOrDefault().Type.Name);
                Assert.AreEqual(
                    tonerLevel, room.Printers.FirstOrDefault().Statistic.TonerLevel);
                Assert.AreEqual(
                    drumLevel, room.Printers.FirstOrDefault().Statistic.DrumLevel);
                Assert.AreEqual(
                    totalPages, room.Printers.FirstOrDefault().Statistic.TotalPagesPrinted);
                Assert.AreEqual(historyRoomName, room.RoomHistory.Name);
            }
        }
        [TestCase("TestCityTestCityTestCityTestCityTestCityTestCityTestCityTestCity")]
        public void DbDataSeed_IsDataRestrictionsEnabled_True(string cityName)
        {
            // Feeding with test data.
            using (var context = new TestDbContext(options))
            {
                Model.City city = new Model.City()
                {
                    Name = cityName,
                };

                context.Cities.Add(city);
                
                Assert.Throws<
                    DbUpdateException>(delegate { context.SaveChanges(); });
            }
        }
    }
}