using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WorkSchedule.Forms;

namespace WorkSchedule
{
    public partial class MainForm : MetroFramework.Forms.MetroForm
    {

        DayForm[,] dayForms = new DayForm[5, 7];
        DayForm[] flattenDayForms = new DayForm[35];
        ScheduleManager SM;
        DateTime firstDay;
        int firstDayofWeek;

        List<(DateTime date, string name)> holidayInfo;

        public MainForm()
        {
            InitializeComponent();
            var count = 0;
            for(int i = 0; i < dayForms.GetLength(0); i++)
            {
                for(int j = 0; j < dayForms.GetLength(1); j++)
                {
                    dayForms[i, j] = new DayForm(calendarLayoutPanel, j, i+1);
                    flattenDayForms[count] = dayForms[i, j];
                    count++;
                }
            }

            var fi = new FileInfo(Constant.SmPath);
            if (fi.Exists)
            {
                ScheduleManager.LoadJson(File.ReadAllText(Constant.SmPath));
            }
            else
            {
                var now = DateTime.Now;
                ScheduleManager.Initialize(now.Year, now.Month);
            }

            SM = ScheduleManager.Instance;
            firstDay = new DateTime(SM.Year, SM.Month, 1);
            firstDayofWeek = (int)firstDay.DayOfWeek;
            holidayInfo = SM.HolidayInfo;
            SM.holidayChange += HolidayChanged;
            Clear();
        }

        public void HolidayChanged(object sender, EventArgs e)
        {
            Clear();
        }
        private int DateToIdx(DateTime date)
        {
            if(firstDayofWeek == 6)
            {
                return (date - firstDay).Days - 1;
            }

            return (date - firstDay).Days + firstDayofWeek;
        }
        private DateTime IdxToDate(int idx)
        { // idx = dayOfWeek + firstDay - Date => Date = dayOfweek + firstDay - idx
            if(firstDayofWeek == 6)
            {
                return firstDay + new TimeSpan(idx + 1, 0, 0, 0);
            }

            return firstDay + new TimeSpan(idx - firstDayofWeek, 0, 0, 0);
        }

        private MainForm Clear()
        {
            (bool value, string name) IsHoliday(DateTime date)
            {
                var dayOfWeek = date.DayOfWeek;
                for (int i = 0; i < holidayInfo.Count; i++)
                {
                    if (holidayInfo[i].date == date)
                    {
                        return (true, holidayInfo[i].name);
                    }
                }
                if (dayOfWeek == DayOfWeek.Sunday)
                {
                    return (true, "");
                }
                


                return (false, "");
            }

            for (int i=0; i<flattenDayForms.Length; i++)
            {
                var date = IdxToDate(i);
                
                var holiday = IsHoliday(date);
                if(holiday.value)
                {
                    flattenDayForms[i].SetHoliday(holiday.name);
                }else if(date.DayOfWeek == DayOfWeek.Saturday)
                {
                    flattenDayForms[i].SetSaturday();
                }else
                {
                    flattenDayForms[i].Clear();
                }
                flattenDayForms[i].SetDateLabel(date, SM.Month);

            }
            BlocksListView.Clear();
            return this;
        }

        

        private void metroButton1_Click(object sender, EventArgs e)
        {
            var workerSettingForm = new WorkerSettingForm();
            if (workerSettingForm.ShowDialog() == DialogResult.OK)
            {

            }
            workerSettingForm.Dispose();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            var workerSettingForm = new CreateForm();
            if (workerSettingForm.ShowDialog() == DialogResult.OK)
            {
                if((SM.Month != workerSettingForm.Month || SM.Year != workerSettingForm.Year) || SM == null)
                {
                    ScheduleManager.Initialize(workerSettingForm.Year, workerSettingForm.Month, null);
                }
           
                SM = ScheduleManager.Instance;
                SM.holidayChange += HolidayChanged;
                firstDay = new DateTime(SM.Year, SM.Month, 1);
                firstDayofWeek = (int)firstDay.DayOfWeek;
                holidayInfo = SM.HolidayInfo;
                Clear();

                SM.FindBestBlocks(workerSettingForm.firstDayOff);
                BlocksListView.Items.Clear();
                for(int i=0; i< SM.BestBlockList.Count; i++)
                {
                    BlocksListView.Items.Add(new ListViewItem($"시간표 {i+1}, {SM.BestBlockList[i].Loss}, {SM.BestBlockList[i].Score}, {SM.BestBlockList[i].WorkCount[0]}, {SM.BestBlockList[i].WorkCount[1]}, {SM.BestBlockList[i].WorkCount[2]}"));
                }
                BlocksListView.Refresh();
                if (BlocksListView.Items.Count > 0)
                {
                    BlocksListView.Items[0].Selected = true;
                }
                ShowSchedule(0);
                
                if(SM.BestBlockList[0].Loss > 0)
                {
                    MessageBox.Show("");
                }


            }
            workerSettingForm.Dispose();
        }

        

        public void ShowSchedule(int idx)
        {
            var days = SM.BestBlockList[idx].Days;
            foreach(var day in days)
            {
                int i = DateToIdx(day.Date);
                flattenDayForms[i].SetWorkLabel(day);
            }
        }

        

        private void BlocksListView_ItemActivate(object sender, EventArgs e)
        {
            
            if (BlocksListView.Items.Count <= 0)
            {
                return;
            }
            var idx = BlocksListView.Items.IndexOf(BlocksListView.SelectedItems[0]);
            ShowSchedule(idx);
        }
    }
}
