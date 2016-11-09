﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SynthBasics
{
    /// <summary>
    /// Interaction logic for Potentiometer.xaml
    /// </summary>
    public partial class Potentiometer : UserControl
    {
        public Potentiometer()
        {
            InitializeComponent();
        }

        public double CurrentSetting
        {
            get { return (double)GetValue(CurrentSettingProperty); }
            set { SetValue(CurrentSettingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentSetting.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentSettingProperty =
            DependencyProperty.Register("CurrentSetting", typeof(double), typeof(Potentiometer), new UIPropertyMetadata(10.0, new PropertyChangedCallback(CurrentSettingChanged)), 
                                        new ValidateValueCallback(ValidateCurrentSetting));

        public static bool ValidateCurrentSetting(object value)
        {
            if (Convert.ToDouble(value) >= 0 && Convert.ToDouble(value) <= 100)
                return true;

            return false;
        }

        /*
        private static void CurrentSettingChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            Potentiometer c = (Potentiometer)depObj;

            Label theLabel = c.numberDisplay;

            theLabel.Content = args.NewValue.ToString();

            double proportion = (double)(args.NewValue) / 100.0;

            c.thumb.SetValue(Canvas.LeftProperty, proportion * c.sliderbkgnd.ActualWidth);
        }
         */ 

        private static void CurrentSettingChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            Potentiometer c = (Potentiometer)depObj;
            Point center = new Point { X = 200, Y = 200 };
            double radius = 175;
            double angleinradians;
            Point newpos;
            double PI = 3.1415927;

            angleinradians = (2.65 * ((double) args.NewValue) - 225) * PI / 180.0;

            newpos = new Point(center.X + radius * Math.Cos(angleinradians), center.Y + radius * Math.Sin(angleinradians));

            //subtract half the width and height of the dot to place the top-left corner
            newpos.X -= 16.3333;
            newpos.Y -= 16.3333;

            c.thumb.SetValue(Canvas.LeftProperty, newpos.X);
            c.thumb.SetValue(Canvas.TopProperty, newpos.Y);
        }
        
    }
}
