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
using System.Diagnostics;

namespace WorkSchedule
{

    [Serializable]
    public class ScheduleManager
    {
        public event EventHandler holidayChange;


        Blocks blocks = null;
        public BestBlockList BestBlockList { private set; get; }
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

        public ScheduleManager FindBestBlocks(int firstDayOff, int iter = 5)
        {

            /***
             첫날 데이터 받음 -> BestFilledBlock Serach
            -> 상위 n개만 구함 (가장 낮은 loss들중에서 best score를 기록한 것)
            -> 다시 상위 n개에서 loss가 0이 될때까지 최대 k번 permute
            -> 존재하지 않으면 error 메세지 출력 존재하면 ListView에 출력
             ***/
            var result = new BestBlockList(10);
            
            var leftHead = new Node(blocks.Clone());
            var rightHead = new Node(blocks.Clone());

            var firstLeftWorkTypes = new List<Day.WorkType>(new Day.WorkType[] { Day.WorkType.Day, Day.WorkType.Night });
            var firstRightWorkTypes = new List<Day.WorkType>(new Day.WorkType[] { Day.WorkType.Night, Day.WorkType.Day });
            firstLeftWorkTypes.Insert(firstDayOff, Day.WorkType.Dayoff);
            firstRightWorkTypes.Insert(firstDayOff, Day.WorkType.Dayoff);

            leftHead.Blocks.SetFirstDay(0, firstLeftWorkTypes.ToArray());
            rightHead.Blocks.SetFirstDay(0, firstRightWorkTypes.ToArray());

            var leftResult = Node.WaterTree(leftHead);
            var rightResult = Node.WaterTree(rightHead);

            Debug.WriteLine(rightResult[0] == rightResult[1]);
            
            var concatResult = leftResult.Concat(rightResult);
            foreach(var node in concatResult)
            {
                result.Add(node.Blocks.Clone());
            }

            BestBlockList Step(BestBlockList blockList)
            {
                if (blockList.Loss == 0) return blockList;
                var permuteResult = new BestBlockList(10);
                foreach (var block in blockList)
                {
                    
                    var count = 0;
                    while (count < 5)
                    {
                        var permute = block.PermuteByBestWay();
                        if (permute.Count == 0) break;
                        foreach (var p in permute)
                        {
                            permuteResult.Add(p.Clone());
                        }
                        count += 1;
                    }

                }
                return permuteResult;
            }

            for(int i=0; i<iter; i++)
            {
                result = Step(result);
            }

            BestBlockList = result;
            return this;

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

        public static ScheduleManager AddHoliday(string name, DateTime date)
        {
            
            var blocks = Instance.blocks;
            var blockIdx = -1;
            var dayIdx = -1;
            for(int i = 0; (i < blocks.Length) && (blockIdx == -1) && (dayIdx == -1); i++)
            {
                var block = blocks.GetBlock(i);
                for (int j = 0; j < block.Length; j++)
                {
                    
                    if(block.Days[j].Date == date)
                    {
                        blockIdx = i;
                        dayIdx = j;
                        break;
                    }
                }
            }

            if(Instance.HolidayInfo.Any(x => x.date == date))
            {
                Instance.HolidayInfo.RemoveAll(x => x.date == date);
                Instance.HolidayInfo.Add((date, name));
                Instance.holidayChange(Instance, EventArgs.Empty);
            }
            else
            {
                blocks.SplitBlock(blockIdx, dayIdx);
                Instance.blocks = blocks;
                Instance.HolidayInfo.Add((date, name));
                Instance.holidayChange(Instance, EventArgs.Empty);
            }
            
            
            return Instance;
        }

        
    }
}
