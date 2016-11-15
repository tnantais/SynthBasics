using System;
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
        static private readonly Point _center = new Point { X = 200, Y = 200 };

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

        private static void CurrentSettingChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            Potentiometer c = (Potentiometer)depObj;
            double radius = 175; //circular arc has overall diameter of 370, but that includes a stroke width of 20 pixels
            double angleinradians;
            Point newpos;

            //convert a value from 0 to 100 to an angle from -225 to +45 (remembering that +ve X and Y are down and to the right)
            angleinradians = (2.7 * ((double) args.NewValue) - 225) * Math.PI / 180.0;

            newpos = new Point(_center.X + radius * Math.Cos(angleinradians), _center.Y + radius * Math.Sin(angleinradians));

            //subtract half the width and height of the dot to place the top-left corner
            newpos.X -= c.thumb.Width / 2;
            newpos.Y -= c.thumb.Height / 2;

            c.thumb.SetValue(Canvas.LeftProperty, newpos.X);
            c.thumb.SetValue(Canvas.TopProperty, newpos.Y);
        }

        private void Path_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition((IInputElement) this); //get position relative to pot
            double newsetting = ConvertRingPositionToPotSetting(pos);
            this.CurrentSetting = newsetting;
        }

        private double ConvertRingPositionToPotSetting(Point posInRing)
        {
            double potvalue = 0;

            //The angle range runs from 45 (fully CW) to -225 (fully CCW) because Y increases downward
            double angleinradians = Math.Atan2(posInRing.Y - _center.Y, posInRing.X - _center.X);
            double angleindegrees = angleinradians * 180 / Math.PI;
            if (angleindegrees > 45)
            {
                angleindegrees -= 360;
            }

            potvalue = ((angleindegrees + 225) / 270) * 100; 

            return potvalue;
        }


        
    }
}
