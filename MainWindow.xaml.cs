using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;

namespace afkCheck0r
{
    public partial class MainWindow : MetroWindow
    {
        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("user32")]
        public static extern void LockWorkStation();

        public Boolean running = false;
        public static System.Timers.Timer aTimer;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = null;
        //public event TextChangedEventHandler TextChanged;
        public static int i = 60;
        public static String  test = "";
        System.DateTime start = System.DateTime.Now;
        System.DateTime now = System.DateTime.Now;
        System.TimeSpan duration;
        DateTime final;


        public Point Location { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            // TextChanged += new TextChangedEventHandler(counterDisplay_TextChanged);
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;

            if (!running)
            {
                lblTime.Content = i + " s";
                lblTime.FontSize = 54;
                start = System.DateTime.Now; 
                duration = new System.TimeSpan(0, 0, 1, 0);
                final = System.DateTime.Now.AddSeconds(10);
                dispatcherTimer = null;
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();
                running = true;
                return;
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            test = i.ToString();

            lblTime.Content = i + " s";
            i--;

            if(running)
            {
                if (System.DateTime.Now >= final)
                {
                    doStuff();
                    System.Windows.Application.Current.Shutdown();
                }
            }
            
        }

        private static void doStuff()
        {
            Application.SetSuspendState(PowerState.Hibernate, true, true);
            // Shutdown: Process.Start("shutdown", "/s /t 0");
            // Hibernate: Application.SetSuspendState(PowerState.Hibernate, true, true);
            // Logscreen: LockWorkStation();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!running)
            {
                this.startStopBtn.Content = "STOP";
                final = System.DateTime.Now.AddMinutes(1);
                dispatcherTimer = null;
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();
                running = true;
                return;
            }

            if(running)
            {
                this.startStopBtn.Content = "START";
                final = DateTime.Now.AddHours(2);
                dispatcherTimer.Stop();
                dispatcherTimer = null;
                i = 60;
                lblTime.Content = i + " s";
                running = false;
            }
        }

        private void MetroWindow_KeyDown_1(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (running)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void startStopBtn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.startStopBtn.Background = Brushes.DarkGray;
        }

        private void startStopBtn_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.startStopBtn.Background = Brushes.LightGray;
        }
    }
}