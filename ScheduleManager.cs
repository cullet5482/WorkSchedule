using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Date;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Xml;


namespace WorkSchedule
{

    [Serializable]
    public class ScheduleManager
    {
        Blocks blocks = null;
        List<(DateTime date, string name)> holidayInfo = null;
        int month;
        int year;

        public List<(DateTime date, string name)> HolidayInfo { get { return holidayInfo; } }
        public int Month { get { return month; }}
        public int Year { get { return year; }}
        
        public List<(DateTime date, WorkerData workerData)> DayData { get
            {
                return null;

            } }
        
        

        private static ScheduleManager scheduleManager;
        public static ScheduleManager Instance
        { get
            {
                if (scheduleManager == null)
                {
                    throw new Exception("The scheduleManager is must be initailized before used");
                }
                return scheduleManager;
            } }

        private static List<(DateTime date, string name)> GetHolidayInfo(int year, int month)
        {   



            string url = "http://apis.data.go.kr/B090041/openapi/service/SpcdeInfoService/getRestDeInfo"; // URL
            url += $"?ServiceKey=" + "qLiQ%2FmNSD3KtfaPzonlaOm7kV8VCdkAZz3tpO%2BiJ1Y4wnfuf2QMaKjpz2irRTouz%2FHpr6xuwF%2FnVwdCRq7lP%2Bw%3D%3D"; // Service Key
            url += $"&solYear={year}";
            url += $"&solMonth={month}";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            string results = string.Empty;
            HttpWebResponse response;
            using (response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();
            }

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(results);
            XmlNodeList itemList = xml.GetElementsByTagName("item");

            var result = new List<(DateTime date, string name)>();
            foreach(XmlNode item in itemList)
            {
                result.Add((DateTime.ParseExact(item["locdate"].InnerText, "yyyyMMdd", null), item["dateName"].InnerText));
            }

            return result;
        }

        public static ScheduleManager LoadJson(string json)
        {
            scheduleManager = JsonConvert.DeserializeObject<ScheduleManager>(json);
            return Instance;
        }

        public static ScheduleManager Save()
        {
            string json = JsonConvert.SerializeObject(Instance);
            File.WriteAllText(Constant.SmPath, json);
            return Instance;
        }


        public static ScheduleManager Initialize(int year, int month, List<(DateTime date, string name)> holidayInfo = null)
        {
            scheduleManager = new ScheduleManager();

            if(holidayInfo == null)
                holidayInfo = GetHolidayInfo(year, month);

            Instance.holidayInfo = holidayInfo;
            Instance.month = month;
            Instance.year = year;

            

            bool IsHoliday(DateTime date)
            {
                var dayOfWeek = date.DayOfWeek;
                return (dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday) || holidayInfo.Exists(holiday => holiday.date == date);
            }

            var startDate = new DateTime(year, month, 1);
            var days = new List<Day>();

            int firstWeek = startDate.GlobalWeek();
            bool indep = true;
            var latestWeek = firstWeek;
            var daysInMonth = DateTime.DaysInMonth(year, month);

            for (int day = 1; day-1 < daysInMonth; day++)
            {
                var date = new DateTime(year, month, day);
                if(IsHoliday(date))
                {
                    indep = true;
                    continue;
                }
                days.Add(new Day(date, indep));
                if (days.Count > 2 && latestWeek != days[days.Count-1].Week)
                {
                    days[days.Count - 2].Last = true;
                }
                latestWeek = days[days.Count- 1].Week;
                indep = false;
            }

            var blockList = new List<Block>();
            var blockDays = new List<Day>();

            for(int i = days.Count-1; i >= 0; i--)
            {
                var day = days[i];
                blockDays.Add(day);
                if(day.Indep)
                {
                    blockDays.Reverse();
                    blockList.Add(new Block(blockDays));
                    blockDays = new List<Day>();
                }
            }

            blockList.Reverse();
            Instance.blocks = new Blocks(blockList);
            return Instance;
        }

        
    }
}
