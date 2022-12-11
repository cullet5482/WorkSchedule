using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace WorkSchedule
{
    [Serializable]
    public struct UserData
    {

    }

    [Serializable]
    public struct WorkerData
    {
        public string name;
        public int daySequence;

        public WorkerData(string name, int daySequence)
        {
            this.name = name;
            this.daySequence = daySequence;
        }

        
        

    }

    public class WorkerDataManager
    {
        public WorkerData[] Datas { get; set; }
        private static WorkerDataManager instance;
        public static WorkerDataManager Instance { get { if (instance == null) instance = new WorkerDataManager(); return instance; } }

        

        public WorkerDataManager()
        {
            var fi = new FileInfo(Constant.WDPath);
            if (fi.Exists)
            {
                var json = File.ReadAllText(Constant.WDPath);
                Datas = JsonConvert.DeserializeObject<WorkerData[]>(json);
            }
            else
            {
                Datas = new WorkerData[] { new WorkerData("A", 0), new WorkerData("B", 1), new WorkerData("C", 2) };
            }
            
        }
        
        public static WorkerDataManager Save()
        {
            string json = JsonConvert.SerializeObject(Instance.Datas);
            File.WriteAllText(Constant.WDPath, json);
            return Instance;
        }

        

        public static string GetWorkerName(int idx)
        {
            return Instance.Datas[idx].name;
        }
        public static int GetDayWorker(int globalWeek)
        {
            var workerDatas = Instance.Datas.Select(x => x.daySequence).ToArray();
            return Array.IndexOf(workerDatas, globalWeek % 3);
        }

    }


}
