﻿using Rubyer;
using RubyerDemo.Consts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RubyerDemo.Views
{
    /// <summary>
    /// Button.xaml 的交互逻辑
    /// </summary>
    public partial class ButtonDemo : UserControl
    {
        public ButtonDemo()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialogBox = Dialog.Show(ConstNames.MainDialogBox, new Loading { Text = "hello~" }, showCloseButton: false);
            await Task.Delay(2000);
            dialogBox.Close();
        }
    }
}
