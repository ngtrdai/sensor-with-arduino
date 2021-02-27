﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO.Ports;
using System.Threading;
using System.Windows.Threading;

namespace DocGhiNhietDoTuArduino
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        SerialPort serialPort = new SerialPort();
        DispatcherTimer timerRealTime = new DispatcherTimer();
        DispatcherTimer timerReviceData = new DispatcherTimer();
        DateTime RealDate = new DateTime();
        public MainWindow()
        {
            InitializeComponent();
            timerRealTime.Tick += TimerRealTime_Tick;
            timerRealTime.Start();
            timerReviceData.Tick += TimerReviceData_Tick;
            timerReviceData.Start();
        }

        private void TimerReviceData_Tick(object sender, EventArgs e)
        {
            if (serialPort.IsOpen == false)
            {
                trangThaiKetNoi.Text = "Chưa kết nối";                   
            }
            else if (serialPort.IsOpen)
            {
                trangThaiKetNoi.Text = "Đã kết nối";
                string nhietDoNhan = serialPort.ReadLine().ToString();
                nhietDo.Text = nhietDoNhan;
            }
        }

        private void TimerRealTime_Tick(object sender, EventArgs e)
        {
            realTime.Text = DateTime.Now.ToShortTimeString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RealDate = DateTime.Now;
            realDate.Text = RealDate.ToShortDateString();
            string[] danhSachCongCOM = SerialPort.GetPortNames();

            foreach (string item in danhSachCongCOM)
            {
                comList.Items.Add(item);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(comList.Text == "")
            {
                MessageBox.Show("Xin vui lòng chọn cổng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                    btnKetNoi.Content = "Kết nối";
                    comList.IsEnabled = true;   
                }
                else
                {
                    try
                    {
                        serialPort.PortName = comList.Text;
                        serialPort.BaudRate = 9600;
                        serialPort.Open();
                        trangThaiKetNoi.Text = "Ngắt kết nối";
                        comList.IsEnabled = false;
                        serialPort.Write("9");
                    }
                    catch
                    {
                        MessageBox.Show("Không thể mở cổng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            

        }
    }
}
