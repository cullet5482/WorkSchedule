using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace WorkSchedule
{
    public partial class WorkerSettingForm : MetroFramework.Forms.MetroForm
    {
        MetroRadioButton[,] dayWorkRadioButtons = new MetroRadioButton[3, 3];
        int[] checkedRadio = new int[] { 0, 1, 2 };
        bool changing = false;

        public WorkerSettingForm()
        {



            InitializeComponent();

            var workerDatas = WorkerDataManager.Instance.Datas;
            workerName1.Text = workerDatas[0].name;
            workerName2.Text = workerDatas[1].name;
            workerName3.Text = workerDatas[2].name;

            dayWorkRadioButtons[0, 0] = dayworkRadioButton1;
            dayWorkRadioButtons[0, 0].CheckedChanged += new EventHandler((sender, e) => RadiobuttonCheckedChanged(sender, e, 0, 0));
            dayWorkRadioButtons[0, 1] = dayworkRadioButton2;
            dayWorkRadioButtons[0, 1].CheckedChanged += new EventHandler((sender, e) => RadiobuttonCheckedChanged(sender, e, 0, 1));
            dayWorkRadioButtons[0, 2] = dayworkRadioButton3;
            dayWorkRadioButtons[0, 2].CheckedChanged += new EventHandler((sender, e) => RadiobuttonCheckedChanged(sender, e, 0, 2));
            dayWorkRadioButtons[1, 0]= dayworkRadioButton4;
            dayWorkRadioButtons[1, 0].CheckedChanged += new EventHandler((sender, e) => RadiobuttonCheckedChanged(sender, e, 1, 0));
            dayWorkRadioButtons[1, 1]= dayworkRadioButton5;
            dayWorkRadioButtons[1, 1].CheckedChanged += new EventHandler((sender, e) => RadiobuttonCheckedChanged(sender, e, 1, 1));
            dayWorkRadioButtons[1, 2] = dayworkRadioButton6;
            dayWorkRadioButtons[1, 2].CheckedChanged += new EventHandler((sender, e) => RadiobuttonCheckedChanged(sender, e, 1, 2));
            dayWorkRadioButtons[2, 0] = dayworkRadioButton7;
            dayWorkRadioButtons[2, 0].CheckedChanged += new EventHandler((sender, e) => RadiobuttonCheckedChanged(sender, e, 2, 0));
            dayWorkRadioButtons[2, 1] = dayworkRadioButton8;
            dayWorkRadioButtons[2, 1].CheckedChanged += new EventHandler((sender, e) => RadiobuttonCheckedChanged(sender, e, 2, 1));
            dayWorkRadioButtons[2, 2] = dayworkRadioButton9;
            dayWorkRadioButtons[2, 2].CheckedChanged += new EventHandler((sender, e) => RadiobuttonCheckedChanged(sender, e, 2, 2));

            dayWorkRadioButtons[0, workerDatas[0].daySequence].Checked = true;
            dayWorkRadioButtons[1, workerDatas[1].daySequence].Checked = true;
            dayWorkRadioButtons[2, workerDatas[2].daySequence].Checked = true;



        }

        private void RadiobuttonCheckedChanged(object sender, EventArgs e, int i, int j)
        {
            if(changing)
            {
                return;
            }
            else
            {
                changing = true;
            }
            var changeTarget = Array.IndexOf(checkedRadio, j);
            dayWorkRadioButtons[changeTarget, checkedRadio[i]].Checked = true;
            dayWorkRadioButtons[i, j].Checked = true;
            checkedRadio[changeTarget] = checkedRadio[i];
            checkedRadio[i] = j;
            changing = false;
        }

        private void dayworkRadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            WorkerDataManager.Instance.Datas = new WorkerData[] { new WorkerData(workerName1.Text, checkedRadio[0]), new WorkerData(workerName2.Text, checkedRadio[1]), new WorkerData(workerName3.Text, checkedRadio[2]) };
            WorkerDataManager.Save();
            Close();
        }
    }
}
