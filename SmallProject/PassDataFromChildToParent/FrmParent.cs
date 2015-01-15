using System;
using System.Windows.Forms;

namespace PassDataFromChildToParent
{
    public partial class FrmParent : Form
    {
        public string ParentText
        {
            get { return this.textBox1.Text; }
            set { this.textBox1.Text = value; }
        }

        public FrmParent()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmChild frm = new FrmChild(this);
            frm.ShowDialog();
            //1.直接访问控件值
            //this.textBox2.Text = (frm.Controls["monthCalendar1"] as MonthCalendar).SelectionStart.ToShortDateString();
            //2.访问属性值
            this.textBox2.Text = frm.ChileDate.ToShortDateString();
        }
    }
}
