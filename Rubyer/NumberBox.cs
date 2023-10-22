﻿using Rubyer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Rubyer
{
    [TemplatePart(Name = TextBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = ButtonIncreasePartName, Type = typeof(ButtonBase))]
    [TemplatePart(Name = ButtonDecreasePartName, Type = typeof(ButtonBase))]
    public class NumberBox : Control
    {
        public const string TextBoxPartName = "PART_TextBox";
        public const string ButtonIncreasePartName = "PART_IncreaseButton";
        public const string ButtonDecreasePartName = "PART_DecreaseButton";

        private TextBox textBox;
        private ButtonBase increaseButton;
        private ButtonBase decreaseButton;

        static NumberBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(typeof(NumberBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(TextBoxPartName) is TextBox textBox)
            {
                var textBinding = new Binding(nameof(Text));
                textBinding.Source = this;
                textBinding.Mode = BindingMode.TwoWay;
                textBox.SetBinding(TextBox.TextProperty, textBinding);
                this.textBox = textBox;
            }

            if (GetTemplateChild(ButtonIncreasePartName) is ButtonBase upButton)
            {
                upButton.Click += IncreaseButton_Click;
                increaseButton = upButton;
            }

            if (GetTemplateChild(ButtonDecreasePartName) is ButtonBase downButton)
            {
                downButton.Click += DecreaseButton_Click;
                decreaseButton = downButton;
            }
        }

        #region events

        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double?>), typeof(NumberBox));

        public event RoutedPropertyChangedEventHandler<double?> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        #endregion

        #region propteries

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
           "Text", typeof(string), typeof(NumberBox), new PropertyMetadata(null, OnTextChanged));

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
           "Value", typeof(double?), typeof(NumberBox), new FrameworkPropertyMetadata(default(double?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        /// <summary>
        /// 值
        /// </summary>
        public double? Value
        {
            get { return (double?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
           "MinValue", typeof(double), typeof(NumberBox), new PropertyMetadata(default(double)));

        /// <summary>
        /// 值
        /// </summary>
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
           "MaxValue", typeof(double), typeof(NumberBox), new PropertyMetadata(default(double)));

        /// <summary>
        /// 值
        /// </summary>
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }


        public static readonly DependencyProperty ShowButtonProperty = DependencyProperty.Register(
           "ShowButton", typeof(bool), typeof(NumberBox), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否显示增减按钮
        /// </summary>
        public bool ShowButton
        {
            get { return (bool)GetValue(ShowButtonProperty); }
            set { SetValue(ShowButtonProperty, value); }
        }

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(
          "MaxLength", typeof(int), typeof(NumberBox), new PropertyMetadata(default(int)));

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(
          "Interval", typeof(double), typeof(NumberBox), new PropertyMetadata(default(double)));

        /// <summary>
        /// 增减间隔
        /// </summary>
        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        public static readonly DependencyProperty NumberTypeProperty = DependencyProperty.Register(
          "NumberType", typeof(NumberType), typeof(NumberBox), new PropertyMetadata(default(NumberType)));

        /// <summary>
        /// 数值类型
        /// </summary>
        public NumberType NumberType
        {
            get { return (NumberType)GetValue(NumberTypeProperty); }
            set { SetValue(NumberTypeProperty, value); }
        }
        #endregion

        #region methods

        private static double GetCalculatedValue(NumberBox numberBox, double value)
        {
            if (value > numberBox.MaxValue)
            {
                return numberBox.MaxValue;
            }
            else if (value < numberBox.MinValue)
            {
                return numberBox.MinValue;
            }
            else
            {
                return value;
            }
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            var interval = NumberType == NumberType.Double ? Interval : Math.Round(Interval);
            var min = MinValue == double.MinValue ? 0 : MinValue;
            Value = GetCalculatedValue(this, Value == null ? min + interval : Value.GetValueOrDefault() + interval);
        }

        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            var interval = NumberType == NumberType.Double ? Interval : Math.Round(Interval);
            var min = MinValue == double.MinValue ? 0 : MinValue;
            Value = GetCalculatedValue(this, Value == null ? min : Value.GetValueOrDefault() - interval);
        }


        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numberBox = d as NumberBox;

            if (string.IsNullOrEmpty(numberBox.Text))
            {
                numberBox.Value = null;
                return;
            }
            else if (double.TryParse(numberBox.Text, out double value))
            {
                var newValue = GetCalculatedValue(numberBox, value);
                if (numberBox.Value != newValue)
                {
                    numberBox.Value = newValue;
                    return;
                }
            }

            numberBox.Text = numberBox.Value.ToString();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numberBox = d as NumberBox;
            numberBox.Text = numberBox.Value.ToString();

            var args = new RoutedPropertyChangedEventArgs<double?>((double?)e.OldValue, (double?)e.NewValue);
            args.RoutedEvent = NumberBox.ValueChangedEvent;
            numberBox.RaiseEvent(args);
        }

        #endregion
    }
}
