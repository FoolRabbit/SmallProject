using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelegateCommandSample
{
    public class ViewModel : ViewModelBase
    {
        #region Property

        private DelegateCommand<string> _CalButtonCommand;
        /// <summary>
        /// 命令定义
        /// </summary>
        public DelegateCommand<string> CalButtonCommand
        {
            get { return _CalButtonCommand; }
            set
            {
                _CalButtonCommand = value;
                NotifyPropertyChanged("CalButtonCommand");
            }
        }

        private string _CustomText;
        /// <summary>
        /// 用于文本绑定
        /// </summary>
        public string CustomText
        {
            get { return _CustomText; }
            set
            {
                _CustomText = value;
                NotifyPropertyChanged("CustomText");
            }
        }


        #endregion


        public ViewModel()
        {
            CalButtonCommand = new DelegateCommand<string>(ExecuteFun, CanExecute);
        }

        /// <summary>
        /// 可执行逻辑
        /// </summary>
        /// <param name="para">用于说明CommandParameter用法</param>
        /// <returns></returns>
        private bool CanExecute(string para)
        {
            //这里能获取到CommandParameter，相同的按钮绑定同一个命令，可以根据CommandParameter不同执行不同的方法
            //当文本框内容为参数CommandParameter的值test时能点击按钮
            return CustomText == para;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="para"></param>
        private void ExecuteFun(string para)
        {
            CustomText = "按钮被点击了";
        }
    }
}
