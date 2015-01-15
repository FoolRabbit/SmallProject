using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ThreadSample
{
    public partial class Form1 : Form
    {
        ThreadClass MyThread = new ThreadClass();

        public Form1()
        {
            InitializeComponent();
        }

        #region 线程方法1

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>() { "1", "2", "3" };
            //定义一个线程，ThreadStart委托就是线程需要执行的方法
            ParameterizedThreadStart threadDelegate = new ParameterizedThreadStart(myfun); ;
            Thread td = new Thread(threadDelegate);
            td.Start(list);
        }

        private void myfun(object para)
        {
            List<string> list = para as List<string>;
            //list操作，给原集合字符串加个string开头
            List<string> newList = list.Select(p => "Test" + p).ToList();
            //将集合转为字符串，string.Join的使用
            string s = string.Join(",", newList);
            //在主线程使用委托，执行其中的方法，子线程中无法操作主线程中的控件，要让主线程自己操作。
            MyDel del = new MyDel(DelFun);
            this.BeginInvoke(del, s);
        }

        /// <summary>
        /// 带参数委托
        /// </summary>
        /// <param name="s"></param>
        private delegate void MyDel(string s);

        /// <summary>
        /// 委托的方法
        /// </summary>
        /// <param name="s"></param>
        private void DelFun(string s)
        {
            this.textBox1.Text = s;
        }

        #endregion

        #region 线程方法2

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>() { "1", "2", "3" };

            //定义一个线程，ThreadStart委托就是线程需要执行的方法
            Thread td = new Thread(new ThreadStart(delegate
            {
                //list操作，给原集合字符串加个string开头
                List<string> newList = list.Select(p => "String" + p).ToList();
                //将集合转为字符串，string.Join的使用
                string s = string.Join(",", newList);
                //在主线程使用委托，执行其中的方法，子线程中无法操作主线程中的控件，要让主线程自己操作。
                this.BeginInvoke(new Action(delegate { this.textBox1.Text = s; }));
            }));
            td.Start();
        }

        #endregion

        #region 线程方法3


        /// <summary>
        /// 采用四个线程
        /// <para>1.取出数据</para>
        /// <para>2.将数据在list中操作</para>
        /// <para>3.操作后数据改为一个变量</para>
        /// <para>4.将变量在Textboxt中显示</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            switch (button3.Text)
            {
                case "获取数据":
                    //用计时器判断线程是否接受，结束了就能执行下一步操作
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                    timer.Interval = 1000;
                    timer.Tick += timer_Tick;

                    MyThread = new ThreadClass();
                    MyThread.DataFrom = 3;
                    MyThread.TextObj = textBox1;
                    MyThread.Thread1.Start();

                    textBox1.Text = "";
                    button3.Enabled = false;
                    timer.Start();
                    break;
                case "操作数据":
                    MyThread.Thread2.Start();
                    button3.Enabled = false;
                    break;
                case "转为变量":
                    MyThread.Thread3.Start();
                    button3.Enabled = false;
                    break;
                case "文本显示":
                    MyThread.Thread4.Start();
                    button3.Enabled = false;
                    break;
            }

        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (button3.Enabled == false)
            {
                switch (button3.Text)
                {
                    case "获取数据":
                        if (button3.Enabled == MyThread.Thread1.IsAlive)
                        {
                            button3.Enabled = !MyThread.Thread1.IsAlive;
                            button3.Text = "操作数据";
                        }
                        break;
                    case "操作数据":
                        if (button3.Enabled == MyThread.Thread2.IsAlive)
                        {
                            button3.Enabled = !MyThread.Thread2.IsAlive;
                            button3.Text = "转为变量";
                        }
                        break;
                    case "转为变量":
                        if (button3.Enabled == MyThread.Thread3.IsAlive)
                        {
                            button3.Enabled = !MyThread.Thread3.IsAlive;
                            button3.Text = "文本显示";
                        }
                        break;
                    case "文本显示":
                        if (button3.Enabled == MyThread.Thread4.IsAlive)
                        {
                            button3.Enabled = !MyThread.Thread4.IsAlive;
                            button3.Text = "获取数据";
                            ((System.Windows.Forms.Timer)sender).Stop();
                        }
                        break;
                }
            }
        }

        #endregion

    }

    public class ThreadClass
    {
        #region Fields

        /// <summary>
        /// 数据源
        /// </summary>
        public object DataFrom;

        /// <summary>
        /// 文本框
        /// </summary>
        public Control TextObj;

        /// <summary>
        /// 取得数据
        /// </summary>
        private List<string> DataList;

        /// <summary>
        /// 数据变量
        /// </summary>
        private string VarData;

        #endregion

        #region 线程

        public Thread Thread1;

        public Thread Thread2;

        public Thread Thread3;

        public Thread Thread4;

        #endregion

        public ThreadClass()
        {
            //获取数据
            Thread1 = new Thread(new ThreadStart(delegate
            {
                int count = Convert.ToInt32(DataFrom);
                DataList = new List<string>();
                for (int i = 1; i <= count; i++)
                {
                    DataList.Add(i.ToString());
                }
            }));
            //数据处理
            Thread2 = new Thread(new ThreadStart(delegate
            {
                for (int i = 0; i < DataList.Count; i++)
                {
                    DataList[i] = "Three" + DataList[i];
                }
            }));
            //转为变量
            Thread3 = new Thread(new ThreadStart(delegate
            {
                VarData = string.Join(",", DataList);
            }));
            //文本框显示
            Thread4 = new Thread(new ThreadStart(delegate
            {
                TextObj.BeginInvoke(new Action(delegate { TextObj.Text = VarData; }));
            }));
        }
    }
}
