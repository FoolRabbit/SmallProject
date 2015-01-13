namespace ChangePassword
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using System.Data.OleDb;

    public partial class frmModifyPwd : Form
    {
        #region Fields

        /// <summary>
        /// 数据源连接
        /// </summary>
        private OleDbConnection _Con;

        #endregion

        public frmModifyPwd()
        {
            InitializeComponent();
        }

        #region Methods

        #region EventFun

        /// <summary>
        /// 确定方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtNewPwd.Text != txtConfirmPwd.Text)
            {
                ShowMessage("两次输入的新密码不一致，无法修改密码。");
                return;
            }

            try
            {
                OpenConnection();
                DataTable dt = QueryData(string.Format("select 1 from [User] where [USERNAME]='{0}' and [PASSWORD]='{1}'", txtUserName.Text, txtOldPwd.Text));
                if (dt.Rows.Count < 1)
                {
                    ShowMessage("原用户名和密码错误，无法修改密码。");
                    return;
                }
                if (ExecuteCommand(string.Format("update [User] set [PASSWORD]='{1}' where [USERNAME]='{0}'", txtUserName.Text, txtNewPwd.Text)) == true)
                {
                    ShowMessage("修改密码成功。");
                    return;
                }
                else
                {
                    ShowMessage("修改密码失败。");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// 重置方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtUserName.Clear();
            txtOldPwd.Clear();
            txtNewPwd.Clear();
            txtConfirmPwd.Clear();
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmModifyPwd_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseConnection();
        }

        #endregion

        #region Private

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="msg"></param>
        private void ShowMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        /// <summary>
        /// 打开数据连接
        /// </summary>
        private void OpenConnection()
        {
            if (_Con == null)
            {
                _Con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\MyData.accdb;Persist Security Info=False");
            }
            _Con.Open();
        }

        /// <summary>
        /// 关闭数据连接
        /// </summary>
        private void CloseConnection()
        {
            if (_Con != null)
            {
                if (_Con.State == ConnectionState.Open)
                {
                    _Con.Close();
                }
                _Con = null;
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        private DataTable QueryData(string cmdText)
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmdText, _Con);
            adapter.Fill(dt);
            return dt;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        private bool ExecuteCommand(string cmdText)
        {
            OleDbCommand cmd = new OleDbCommand(cmdText, _Con);
            if (cmd.ExecuteNonQuery() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion


    }
}
