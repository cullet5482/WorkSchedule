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
            Clear();
        }

        private int DateToIdx(DateTime date)
        {
            return (firstDay - date).Days + firstDayofWeek;
        }
        private DateTime IdxToDate(int idx)
        { // idx = dayOfWeek + firstDay - Date => Date = dayOfweek + firstDay - idx
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
                flattenDayForms[i].SetDateLabel(date, SM.Month);
                var holiday = IsHoliday(date);
                if(holiday.value)
                {
                    flattenDayForms[i].SetHoliday(holiday.name);
                }else if(date.DayOfWeek == DayOfWeek.Saturday)
                {
                    flattenDayForms[i].SetSaturday();
                }

            }

            return this;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            var workerSettingForm = new WorkerSettingForm();
            if (workerSettingForm.ShowDialog() == DialogResult.OK)
            {

            }
            workerSettingForm.Dispose();
        }
    }
}
