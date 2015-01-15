using System;
using System.Windows.Forms;

namespace PassDataFromChildToParent
{
    public partial class FrmChild : Form
    {
        private FrmParent _ParentForm;

        public DateTime ChileDate
        {
            get { return this.monthCalendar1.SelectionStart; }
            set { this.monthCalendar1.SelectionStart = value; }
        }

        public FrmChild(FrmParent parent)
        {
            InitializeComponent();

            _ParentForm = parent;
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            //1.设置控件值
            //(_ParentForm.Controls["textBox1"] as TextBox).Text = monthCalendar1.SelectionStart.ToShortDateString();
            //2.设置属性值
            _ParentForm.ParentText = monthCalendar1.SelectionStart.ToShortDateString();
        }
    }
}
