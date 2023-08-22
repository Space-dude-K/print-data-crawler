using data_access_layer.Context;
using data_access_layer.Model.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;

namespace data_access_layer
{
    public class Init
    {
        public void InitDALForPrinters()
        {
            // Gets the current path (executing assembly)
            string currentPath = Path.GetDirectoryName(@"E:\work\Projects\print-data-crawler\");
            // Your DB filename    
            string dbFileName = "PrinterDB.db";
            // Creates a full path that contains your DB file
            string absolutePath = Path.Combine(currentPath, dbFileName);


            string dbName = absolutePath;

            try
            {
                if (File.Exists(dbName))
                {
                    File.Delete(dbName);
                }

                var options = new DbContextOptionsBuilder<PrinterContext>()
                    .UseSqlite($"Filename={Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName, "PrinterDB.db")}")
                    .Options;

                using (var dbContext = new PrinterContext(options))
                {
                    //Ensure database is created
                    // dbContext.Database.EnsureCreated();

                    City city = new City()
                    {
                        Name = "TestCityTestCityTestCityTestCityTestCityTestCityTestCityTestCityTestCityTestCityTestCity",
                        Organizations = new Collection<Organization>()
                    {
                        new Organization()
                        {
                            Name = "TestOrg",
                            Rooms = new Collection<Room>()
                            {
                                new Room()
                                {
                                    Name = "TestRoom",
                                    Printers = new Collection<Printer>()
                                    {
                                        new Printer()
                                        {
                                            Statistic = new Statistic()
                                            {
                                                TonerLevel = 1002,
                                                DrumLevel = 100,
                                                TotalPagesPrinted = 32423
                                            },
                                            Type = new Model.Db.Type()
                                            {
                                                Name = "TestType"
                                            }
                                        }
                                    },
                                    RoomHistory = new RoomHistory()
                                    {
                                        Name = "TestRoomHistory"
                                    }
                                }
                            }
                        }
                    }
                    };


                    dbContext.Cities.Add(city);
                    dbContext.SaveChanges();

                    /*
                    if (!dbContext.Printers.Any())
                    {
                        Model.Db.Type pt = new Model.Db.Type()
                        {
                            Id = 1,
                            Name = "Type1"
                        };
                        Model.Db.Type pt1 = new Model.Db.Type()
                        {
                            Id = 2,
                            Name = "Type2"
                        };

                        Printer printer = new Printer()
                        {
                            Statistic = new Statistic()
                            {
                                TonerLevel = 1002,
                                DrumLevel = 100,
                                TotalPagesPrinted = 32423
                            }

                        };
                        Printer printer1 = new Printer()
                        {
                            Statistic = new Statistic()
                            {
                                DrumLevel = 100,
                                TonerLevel = 100,
                                TotalPagesPrinted = 6666
                            }

                        };
                        Printer printer2 = new Printer()
                        {
                            Statistic = new Statistic()
                            {
                                DrumLevel = 100,
                                TonerLevel = 100,
                                TotalPagesPrinted = 5345
                            }
                        };

                        dbContext.Printers.Add(printer);
                        dbContext.Printers.Add(printer1);
                        dbContext.Printers.Add(printer2);

                        try
                        {
                            dbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            throw;
                        }


                    }
                    
                    foreach (var printer in dbContext.Printers)
                    {
                        Console.WriteLine($"printerId = {printer.Id} " +
                            $"PrinterStatisticId = { printer.Statistic.Id} { printer.Statistic.TonerLevel} { printer.Statistic.TotalPagesPrinted}");
                    }
                    */
                    Console.WriteLine("Done.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}