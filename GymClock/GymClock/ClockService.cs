using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;

namespace GymClock
{
    public static class ClockService
    {
        static string clockDataUri = "http://worldtimeapi.org/api/timezone/America/";

      //  static string weatherJson = @"{""abbreviation"":""PDT"", ""client_ip"":""173.180.233.42"", ""datetime"":""2021-06-27T18:02:48.281131-07:00"", ""day_of_week"":0, ""day_of_year"":178, ""dst"":true, ""dst_from"":""2021-03-14T10:00:00+00:00"", ""dst_offset"":3600, ""dst_until"":""2021-11-07T09:00:00+00:00"", ""raw_offset"":-28800, ""timezone"":""America/Vancouver"", ""unixtime"":1624842168, ""utc_datetime"":""2021-06-28T01:02:48.281131+00:00"", ""utc_offset"":""-07:00"", ""week_number"":25}";

        static ClockService() { }

        public static async Task<DateTime> GetTime()
        {
            string json;

            // uncomment to get live data
            using (HttpClient client = new HttpClient())
            {
                 HttpResponseMessage response = await client.GetAsync(clockDataUri + "Vancouver");
                 json = await response.Content.ReadAsStringAsync();
            } 

            //var json = weatherJson;

            var weatherData = SimpleJsonSerializer.JsonSerializer.DeserializeString(json) as Hashtable;

            var todayString = weatherData["datetime"].ToString();

            var today = DateTime.Parse(todayString);

            return today;
        }
    }
}