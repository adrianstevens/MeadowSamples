using System;
using System.IO;
using Meadow.Units;
using SQLite;

namespace SQLiteNetConsoleTest
{
    class MainClass
    {
        static SQLiteConnection Database { get; set; }

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //test relative humidity
            var humidity = new RelativeHumidity(91, RelativeHumidity.UnitType.Percent);
            Console.WriteLine($"Humidity: {humidity.Percent}");

            // database files should go in the `DataDirectory`
            var databasePath = "ClimateReadings.db";
            // make the connection
            Database = new SQLite.SQLiteConnection(databasePath);

            ConfigureDatabase();

            EraseClimateData();

            AddDBValues();

            ReadDBValues();
        }

        public static void ConfigureDatabase()
        {
            // add table(s)
            Console.WriteLine("ConfigureDatabase");
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

            foreach(var climate in data)
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
