using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FlightSimulatorApp.Views
{
    public partial class Joystick : UserControl
    {
        public double Rudder
        {
            get { return Convert.ToDouble(GetValue(RudderProperty)); }
            set { SetValue(RudderProperty, value); }
        }

        public static readonly DependencyProperty RudderProperty =
            DependencyProperty.Register("Rudder", typeof(double), typeof(Joystick), null);

        public double Elevator
        {
            get { return Convert.ToDouble(GetValue(ElevatorProperty)); }
            set { SetValue(ElevatorProperty, value); }
        }

        public static readonly DependencyProperty ElevatorProperty =
            DependencyProperty.Register("Elevator", typeof(double), typeof(Joystick), null);

        public double DiffFromLastPosition
        {
            get { return Convert.ToDouble(GetValue(DiffFromLastPositionProperty)); }
            set { SetValue(DiffFromLastPositionProperty, value); }
        }

        public static readonly DependencyProperty DiffFromLastPositionProperty =
            DependencyProperty.Register("DiffFromLastPosition", typeof(double), typeof(Joystick), null);

        private Point center;
        private bool isPressed;
        private double newX, newY;
        //members for the Animation
        private readonly Storyboard sb;
        private readonly DoubleAnimation x;
        private readonly DoubleAnimation y;

        public Joystick()
        {
            InitializeComponent();
            center = new Point(Base.Width / 2, Base.Height / 2);
            isPressed = false;
            newX = newY = 0;
            sb = Knob.Resources["CenterKnob"] as Storyboard;
            x = sb.Children[0] as DoubleAnimation;
            y = sb.Children[1] as DoubleAnimation;
            x.From = 0;
            y.From = 0;
        }

        private void CenterKnob_Completed(object sender, EventArgs e)
        {
        }

        private void Knob_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isPressed = true;
            Knob.CaptureMouse();
        }

        private void Knob_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isPressed)
            {
                newX = e.GetPosition(Base).X;
                newY = e.GetPosition(Base).Y;
                //cheak if the knobBase is in Bound
                double bound = Math.Sqrt(Math.Pow(newX - center.X, 2) + Math.Pow(newY - center.Y, 2));
                
                if (bound > (this.Base.Width / 2) - (KnobBase.Width / 2))
                {
                    return;
                }
                else
                {
                    Rudder = (newX - center.X) / (Base.Width / 2 - KnobBase.Width / 2);
                    Elevator = -((newY - center.Y) / (Base.Width / 2 - KnobBase.Width / 2));
                    //the Animation
                    y.To = newY - center.Y;
                    x.To = newX - center.X;
                    double diff = Math.Sqrt(Math.Pow(x.To.GetValueOrDefault() - x.From.GetValueOrDefault(), 2) + Math.Pow(y.To.GetValueOrDefault() - y.From.GetValueOrDefault(), 2));
                    if (diff > 1)
                    {
                        DiffFromLastPosition = diff;
                    }
                    sb.Begin();
                    x.From = x.To;
                    y.From = y.To;
                }
            }
        }

        private void Knob_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isPressed = false;
            Knob.ReleaseMouseCapture();
            //return knobBase to center
            newX = newY = 0;
            x.To = newX;
            y.To = newY;
            sb.Begin();
            x.From = x.To;
            y.From = y.To;
            Rudder = Elevator = 0;
        }
    }
}
