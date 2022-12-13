using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkSchedule
{
    public partial class CreateForm : MetroFramework.Forms.MetroForm
    {

        public int Year { private set; get; }
        public int Month {  private set; get; }
        public int firstDayOff { private set; get; }


        public CreateForm()
        {
            InitializeComponent();
            workerLabel1.Text = WorkerDataManager.GetWorkerName(0);
            workerLabel2.Text = WorkerDataManager.GetWorkerName(1);
            workerLabel3.Text = WorkerDataManager.GetWorkerName(2);
            monthPicker.Value = new DateTime(ScheduleManager.Instance.Year, ScheduleManager.Instance.Month, 1);

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var date = monthPicker.Value;
            Year = date.Year;
            Month = date.Month;
            if(metroRadioButton1.Checked)
            {
                firstDayOff = 0;
            }else if(metroRadioButton2.Checked)
            {
                firstDayOff = 1;
            }else
            {
                firstDayOff = 2;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
