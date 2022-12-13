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
    public partial class HolidayAddForm : MetroFramework.Forms.MetroForm
    {
        public string Name { private set; get; }
        public HolidayAddForm(DateTime date, string name = "", string beforeName = "")
        {
            Name = name;
            InitializeComponent();
            dateLabel.Text = date.ToString("yyyy-MM-dd");
            holidayTextBox.Text = beforeName;

        }

        private void HolidayAddForm_Load(object sender, EventArgs e)
        {

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Name = holidayTextBox.Text;
            Close();
        }
    }
}
