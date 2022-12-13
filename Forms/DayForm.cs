using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkSchedule.Date;
using Day = WorkSchedule.Date.Day;

namespace WorkSchedule.Forms
{
    public class DayForm
    {
        Label dateLabel;
        Label workLabel;
        TableLayoutPanel tableLayoutPanel;

        
        public DayForm(TableLayoutPanel calendarLP, int i, int j)
        {
            dateLabel = new Label();
            dateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            workLabel = new Label();
            workLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.Controls.Add(dateLabel, 0, 0);
            tableLayoutPanel.Controls.Add(workLabel, 0, 1);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Margin = new Padding(0);
            

            calendarLP.Controls.Add(tableLayoutPanel, i, j);
            
            dateLabel.Dock = DockStyle.Fill;
            workLabel.Dock = DockStyle.Fill;


            
            dateLabel.Font = new System.Drawing.Font(dateLabel.Font, System.Drawing.FontStyle.Bold);
            
        }

        public DayForm Clear()
        {
            workLabel.Text = "";
            workLabel.Font = new System.Drawing.Font(workLabel.Font, System.Drawing.FontStyle.Regular);
            tableLayoutPanel.BackColor = System.Drawing.Color.White;
            dateLabel.ForeColor = System.Drawing.Color.Black;
            return this;
        }
        public DayForm SetHoliday(string name)
        {
            tableLayoutPanel.BackColor = System.Drawing.Color.Yellow;

            dateLabel.ForeColor = System.Drawing.Color.Red;
            
            workLabel.Text = name;
            workLabel.Font = new System.Drawing.Font(workLabel.Font, System.Drawing.FontStyle.Bold);
            

            return this;

        }
        public DayForm SetSaturday()
        {
            dateLabel.ForeColor = System.Drawing.Color.Blue;
            return this;
        }

        public DayForm SetWorkLabel(Day day)
        {
            string WorkToStr(Day.WorkType workType)
            {
                switch (workType)
                {
                    case Day.WorkType.Day:
                        return "주간(1)";
                    case Day.WorkType.Dayoff:
                        return "대체(0)";
                    case Day.WorkType.Night:
                        return "야간(2)";
                    default:
                        return "";

                }
            }

            

            string[] worker = new string[] { WorkerDataManager.GetWorkerName(0), WorkerDataManager.GetWorkerName(1), WorkerDataManager.GetWorkerName(2) };
            var workTypes = day.WorkTypes;
            workLabel.Text = $"{worker[0]}: {WorkToStr(workTypes[0])}\n{worker[1]}: {WorkToStr(workTypes[1])}\n{worker[2]}: {WorkToStr(workTypes[2])}";

            return this;
        }

        public DayForm SetDateLabel(DateTime Date, int month)
        {
            if(Date.Month == month)
            {
                dateLabel.Text = Date.Day.ToString();
            }else
            {
                dateLabel.Text = $"{Date.Month}/{Date.Day}";
            }
            
            return this;
        }
    }
}
