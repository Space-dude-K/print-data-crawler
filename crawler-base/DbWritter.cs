using data_access_layer.Context;
using data_access_layer.Model.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using service_layer.Settings;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using web_page_parser;
using Model = data_access_layer.Model.Db;

namespace crawler_base
{
    class DbWritter
    {
        private readonly IConfigurator config;
        private readonly ILogger<ConsoleApp> logger;
        private readonly PrinterContext printerDbContext;
        public DbWritter(IConfigurator config, ILogger<ConsoleApp> logger, PrinterContext printerContext)
        {
            this.logger = logger;
            this.printerDbContext = printerContext;
            this.config = config;

            AddPrinterTypes();
        }

        public DbWritter()
        {

        }

        public void WriteToDb(string cityName, string orgName, string roomName,
            int tonerLevel, int drumLevel, int totalPages,
            string typeName, string historyRoomName)
        {
            City city = AddNewCityToDb(cityName);

            Organization org = AddNewOrganizationToDb(city, orgName);
            Room room = AddNewRoomToDb(org, roomName);

            Model.Type type = printerDbContext.Types.Where(t => t.Name == typeName).FirstOrDefault();
            Statistic stat = new Statistic { TonerLevel = tonerLevel, DrumLevel = drumLevel, TotalPagesPrinted = totalPages };
            AddNewPrinterToDb(room, type, stat);
        }
        public void AddPrinterTypes()
        {
            foreach (var printerTypes in Enum.GetValues(typeof(data_access_layer.Enums.PrinterType)))
            {
                printerDbContext.Types.Add(new Model.Type() { Name = printerTypes.ToString() });
            }

            printerDbContext.SaveChanges();
        }
        public City AddNewCityToDb(string cityName)
        {
            City city = null;

            if (string.IsNullOrEmpty(cityName))
            {
                var ex =
                    new Exception(String.Format("[AddNewCityToDb] Error: Current city name is null or empty: {0}", cityName));

                logger.LogError(ex, "[AddNewCityToDb] Error: Current city name is null or empty: {0}", cityName);
                throw ex;
            }

            if (!printerDbContext.Cities.AsNoTracking().Any(o => o.Name == cityName))
            {
                city = new City();
                city.Name = cityName;

                printerDbContext.Cities.Add(city);
                printerDbContext.SaveChanges();
            }
            else
            {
                logger.LogInformation("[AddNewCityToDb] Error: Current city name alrdy exist: {0}", cityName);
            }

            return city;
        }
        public Organization AddNewOrganizationToDb(City city, string organizationName)
        {
            Organization org = null;

            if (string.IsNullOrEmpty(organizationName))
            {
                var ex =
                    new Exception(String.Format("[AddNewOrganizationToDb] Error: Current organization name is null or empty: {0}", organizationName));

                logger.LogError(ex, "[AddNewOrganizationToDb] Error: Current organization name is null or empty: { 0}", organizationName);
                throw ex;
            }

            if (!printerDbContext.Organizations.AsNoTracking().Any(o => o.Name == organizationName))
            {

                int newOrgId = 0;
                newOrgId = printerDbContext.Organizations.AsNoTracking().Any() ?
                    (int)printerDbContext.Organizations.AsNoTracking().Max(o => o.Id) + 1 : newOrgId + 1;

                org = new Organization()
                {
                    Name = organizationName,
                    City = city
                };

                printerDbContext.Organizations.Add(org);
                printerDbContext.SaveChanges();
            }
            else
            {
                logger.LogInformation("[AddNewOrganizationToDb] Error: Current organization name alrdy exist: {0}", organizationName);
            }

            return org;
        }
        public Room AddNewRoomToDb(Organization org, string roomName)
        {
            Room room = null;

            if (string.IsNullOrEmpty(roomName))
            {
                var ex =
                    new Exception(String.Format("[AddNewRoomToDb] Error: Current room name is null or empty: {0}", roomName));

                logger.LogError(ex, "[AddNewRoomToDb] Error: Current room name is null or empty: { 0}", roomName);
                throw ex;
            }

            if (!printerDbContext.Rooms.AsNoTracking().Any(o => o.Name == roomName))
            {
                room = new Room();
                room.Name = roomName;
                room.Organization = org;

                printerDbContext.Rooms.Add(room);
                printerDbContext.SaveChanges();
            }
            else
            {
                room = printerDbContext.Rooms.Where(r => r.Name == roomName).FirstOrDefault();
                logger.LogInformation("[AddNewRoomToDb] Error: Current room name alrdy exist: {0}", roomName);
            }

            return room;
        }
        public void AddNewPrinterToDb(Room room, Model.Type type, Statistic statistic)
        {
            Printer printer = new Printer()
            {
                Type = type,
                Statistic = statistic,
                Room = room
            };

            printerDbContext.Printers.Add(printer);
            printerDbContext.SaveChanges();
        }
        public void AddStatisticToDb(int printerId, int tonerLevel, int drumLevel, int totalPages)
        {
            if (printerDbContext.Printers.AsNoTracking().Any(p => p.Id == printerId))
            {
                var ex =
                    new Exception(String.Format("[AddStatisticToDb] Error: Printer not found. Printer id: {0}", printerId));

                logger.LogError(ex, "[AddStatisticToDb] Error: Printer not found. Printer id: {0}", printerId);
                throw ex;
            }
            else
            {
                var stat = new Statistic()
                {
                    TonerLevel = tonerLevel,
                    DrumLevel = drumLevel,
                    TotalPagesPrinted = totalPages
                };

                printerDbContext.Printers.Where(p => p.Id == printerId).FirstOrDefault().Statistic = stat;
            }
        }
    }
}
