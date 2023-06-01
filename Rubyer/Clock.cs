﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Rubyer
{
    [TemplatePart(Name = HourListPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = MinuteListPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = SecondListPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = ConfirmPartName, Type = typeof(Button))]
    [TemplatePart(Name = SelectTimePartName, Type = typeof(TextBlock))]
    public class Clock : Control
    {
        public const string HourListPartName = "PART_HourList";
        public const string MinuteListPartName = "PART_MinuteList";
        public const string SecondListPartName = "PART_SecondList";
        public const string ConfirmPartName = "PART_ConfirmButton";
        public const string SelectTimePartName = "PART_SelectTime";

        static Clock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Clock), new FrameworkPropertyMetadata(typeof(Clock)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(SelectTimePartName) is TextBlock selectTimeText)
            {
                var binding = new Binding("DisplayTime");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                binding.StringFormat = "{0:HH : mm : ss}";
                selectTimeText.SetBinding(TextBlock.TextProperty, binding);
            }

            DateTime now = DateTime.Now;

            if (GetTemplateChild(HourListPartName) is ListBox hourList)
            {
                var binding = new Binding("Hour");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                hourList.SetBinding(ListBox.SelectedItemProperty, binding);

                AddItemSource(hourList, 24, now.Hour);
            }

            if (GetTemplateChild(MinuteListPartName) is ListBox minuteList)
            {
                var binding = new Binding("Minute");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                minuteList.SetBinding(ListBox.SelectedItemProperty, binding);

                AddItemSource(minuteList, 60, now.Minute);
            }

            if (GetTemplateChild(SecondListPartName) is ListBox secondList)
            {
                var binding = new Binding("Second");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                secondList.SetBinding(ListBox.SelectedItemProperty, binding);

                AddItemSource(secondList, 60, now.Second);
            }

            if (GetTemplateChild(ConfirmPartName) is Button confirmButton)
            {
                confirmButton.Click += ConfirmButton_Click;
            }
        }

        #region 路由事件
        public static readonly RoutedEvent SelectedTimeChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectedTimeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(Clock));

        public event RoutedPropertyChangedEventHandler<DateTime?> SelectedTimeChanged
        {
            add { AddHandler(SelectedTimeChangedEvent, value); }
            remove { RemoveHandler(SelectedTimeChangedEvent, value); }
        }

        public static readonly RoutedEvent CurrentTimeChangedEvent = EventManager.RegisterRoutedEvent(
            "CurrentTimeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(Clock));

        public event RoutedPropertyChangedEventHandler<DateTime?> CurrentTimeChanged
        {
            add { AddHandler(CurrentTimeChangedEvent, value); }
            remove { RemoveHandler(CurrentTimeChangedEvent, value); }
        }
        #endregion


        #region 依赖属性

        public static readonly DependencyProperty HourProperty = DependencyProperty.Register(
           "Hour", typeof(int?), typeof(Clock), new PropertyMetadata(0, OnListSeletedChanged));

        /// <summary>
        /// 时
        /// </summary>
        public int? Hour
        {
            get { return (int?)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }

        public static readonly DependencyProperty MinuteProperty = DependencyProperty.Register(
           "Minute", typeof(int?), typeof(Clock), new PropertyMetadata(0, OnListSeletedChanged));

        /// <summary>
        /// 分
        /// </summary>
        public int? Minute
        {
            get { return (int?)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); }
        }

        public static readonly DependencyProperty SecondProperty = DependencyProperty.Register(
           "Second", typeof(int?), typeof(Clock), new PropertyMetadata(0, OnListSeletedChanged));

        /// <summary>
        /// 秒
        /// </summary>
        public int? Second
        {
            get { return (int?)GetValue(SecondProperty); }
            set { SetValue(SecondProperty, value); }
        }

        public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(
           "SelectedTime", typeof(DateTime?), typeof(Clock), new PropertyMetadata(default(DateTime)));

        /// <summary>
        /// 选中时间
        /// </summary>
        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        public static readonly DependencyProperty DisplayTimeProperty =
            DependencyProperty.Register("DisplayTime", typeof(DateTime?), typeof(Clock), new PropertyMetadata(default(DateTime?)));

        /// <summary>
        /// 显示时间
        /// </summary>
        public DateTime? DisplayTime
        {
            get { return (DateTime?)GetValue(DisplayTimeProperty); }
            set { SetValue(DisplayTimeProperty, value); }
        }

        #endregion

        /// <summary>
        /// 添加子项
        /// </summary>
        /// <param name="itemsControl">列表控件</param>
        /// <param name="count">总数</param>
        /// <param name="index">当前索引</param>
        private void AddItemSource(ItemsControl itemsControl, int count, int index)
        {
            string[] array = new string[count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i.ToString("D2");
            }

            itemsControl.ItemsSource = array;

            if (itemsControl is ListBox listBox)
            {
                listBox.SelectedIndex = index;
                int scrollIndex = index + 2 > array.Length - 1 ? array.Length - 1 : index + 2;
                listBox.ScrollIntoView(array[scrollIndex]);
            }
        }

        /// <summary>
        /// 时间选择改变
        /// </summary>
        private static void OnListSeletedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var clock = d as Clock;
            clock.DisplayTime = Convert.ToDateTime($"{clock.Hour}:{clock.Minute}:{clock.Second}");

            if (e.NewValue != null)
            {
                RoutedPropertyChangedEventArgs<DateTime?> args = new RoutedPropertyChangedEventArgs<DateTime?>(DateTime.Now, (DateTime)clock.DisplayTime);
                args.RoutedEvent = Clock.CurrentTimeChangedEvent;
                clock.RaiseEvent(args);
            }
        }


        /// <summary>
        /// 确认时间
        /// </summary>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var oldTime = SelectedTime;
            var newTime = DisplayTime;

            SelectedTime = DisplayTime;

            RoutedPropertyChangedEventArgs<DateTime?> args = new RoutedPropertyChangedEventArgs<DateTime?>(oldTime, newTime);
            args.RoutedEvent = Clock.SelectedTimeChangedEvent;
            this.RaiseEvent(args);
        }
    }
}
