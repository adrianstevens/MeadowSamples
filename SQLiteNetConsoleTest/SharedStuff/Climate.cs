using System;
using Meadow.Units;
using SQLite;

namespace SQLiteNetConsoleTest
{
    [Table("ClimateReadings")]
    public class Climate
    {
        [PrimaryKey, AutoIncrement]
        public int? ID { get; set; }

        public double? SpeedValue
        {
            get => Speed.Value.MetersPerSecond;
            set => Speed = new Speed(value.Value, Meadow.Units.Speed.UnitType.MetersPerSecond);
        }

        public double? HumidityValue
        {
            get => Humidity.Value.Percent;
            set => Humidity = new RelativeHumidity(value.Value);
        }

        [Indexed]
        public DateTime DateTime { get; set; }
        /// <summary>
        /// Whether or not this particular reading has been uploaded to the cloud.
        /// </summary>
        public bool Synchronized { get; set; }

        [Ignore]
        public Speed? Speed { get; set; }
        [Ignore]
        public RelativeHumidity? Humidity { get; set; }
    }

    [Serializable()]
    public struct MyCoolType
    {
        public int member1;
        public string member2;
        public string member3;
        public double member4;

        // A field that is not serialized.
        [NonSerialized()]
        public string member5;

        public MyCoolType(int mem1, string mem2, string mem3, double mem4)
        {
            member1 = 11;
            member2 = "hello";
            member3 = "hello";
            member4 = 3.14159265;
            member5 = "hello world!";
        }

        public void Print()
        {

            Console.WriteLine("member1 = '{0}'", member1);
            Console.WriteLine("member2 = '{0}'", member2);
            Console.WriteLine("member3 = '{0}'", member3);
            Console.WriteLine("member4 = '{0}'", member4);
            Console.WriteLine("member5 = '{0}'", member5);
        }
    }
}
