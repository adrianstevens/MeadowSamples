using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Meadow;
using Meadow.Units;
using SQLite;
using SQLiteNetConsoleTest;

namespace MeadowApp
{
    class Program
    {
        static SQLiteConnection Database { get; set; }

        static IApp app;
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello");

            Debug.WriteLine("Hi");

            // database files should go in the `DataDirectory`
            var databasePath = Path.Combine(MeadowOS.FileSystem.DataDirectory, "ClimateReadings.db");
            // make the connection
            Database = new SQLite.SQLiteConnection(databasePath);

            Console.WriteLine("Configure");
            ConfigureDatabase();

          //  Console.WriteLine("Erase");
          //  EraseClimateData();

            Console.WriteLine("Add");
            AddDBValues();

            Console.WriteLine("Read");
            ReadDBValues();

            Thread.Sleep(Timeout.Infinite);
        }

        public static void ConfigureDatabase()
        {
            // add table(s)
            Console.WriteLine("ConfigureDatabase");
            Database.DropTable<Climate>();
            Database.CreateTable<Climate>();
            Console.WriteLine("Table created");
        }

        public static void AddDBValues()
        {
            Database.Insert(new Climate() { DateTime = DateTime.Now, SpeedValue = 5.0, HumidityValue = 0.9 });
            Database.Insert(new Climate() { DateTime = DateTime.Now, SpeedValue = 6.0, HumidityValue = 40.5 });
            Database.Insert(new Climate() { DateTime = DateTime.Now, SpeedValue = 7.0, HumidityValue = 91 });

            Database.Insert(new Climate()
            {
                DateTime = DateTime.Now,
                Speed = new Speed(400),
                Humidity = new RelativeHumidity(91)
            });
            Database.Insert(new Climate()
            {
                DateTime = DateTime.Now,
                Speed = new Speed(40),
                Humidity = new RelativeHumidity(10)
            });
        }

        public static void ReadDBValues()
        {
            var data = Database.Table<Climate>().ToList();

            foreach (var climate in data)
            {
                //Console.WriteLine($"{climate.DateTime}, {climate.SpeedValue}, {climate.HumidityValue}");
                Console.WriteLine($"{climate.DateTime}, {climate.Speed}, {climate.Humidity}");
            }
        }

        public static void EraseClimateData()
        {
            Database.DeleteAll<Climate>();
        }
    }
}
