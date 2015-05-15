using Ags.ProjectorController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Ags.RemoteControl
{
    using Microsoft.Win32;

    public partial class Form1 : Form
    {
        private IProjectorController _controller;
        private SerialPort _serialPort;
        private const string _casioprojectorPowerOn = "(PWR1)";
        private const string _casioprojectorResponsePowerOn = "(0-1,1)";
        private const string _casioprojectorPowerOff = "(PWR0)";
        private const string _casioprojectorResponsePowerOff = "(0-1,0)";
        private const string _casioprojectorPowerQuery = "(PWR?)";
        private const string _prometheanprojectorPowerOn = "~PN\r";
        private const string _prometheanprojectorResponsePowerOn = "On";
        private const string _prometheanprojectorPowerOff = "~PF\r";
        private const string _prometheanprojectorResponsePowerOff = "Off";
        private const string _prometheanprojectorPowerQuery = "~qP\r";
        private string _reply = string.Empty;

        public Form1()
        {
            InitializeComponent();


            var type = typeof(IProjectorController);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.Name.EndsWith("Decorator"));

            var list = new List<Projector>();

            string ComPort = string.Empty;
            string Controller = string.Empty;

            string[] ports = SerialPort.GetPortNames();

            richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " The following Ports where found");

            foreach (string port in ports)
            {
                richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " " + port);
            }

            richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " Attempting to Scan for projectors on the above ports");

            foreach (string port in ports)
            {
                try
                {
                    _serialPort = new SerialPort(port);
                    //Lets see if the current projector is a Casio
                    //State what port i am using
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " Casio Scan - " + port);
                    // Open Serial Port
                    _serialPort.Open();
                    // Lets ask the currect power state so we are not asking the projector to do something it dosnt need
                    _serialPort.Write(_casioprojectorPowerQuery);
                    // Lets wait for the device to respond
                    Thread.Sleep(2);
                    // Put the reply into a string
                    _reply = _serialPort.ReadExisting();
                    if (_reply == _casioprojectorResponsePowerOff)
                    {
                        comboBox1.SelectedItem = list.FirstOrDefault(x => x.DisplayName.StartsWith("Casio"));
                        ComPort = port;
                    }
                    if (_reply == _casioprojectorResponsePowerOn)
                    {
                        comboBox1.SelectedItem = list.FirstOrDefault(x => x.DisplayName.StartsWith("Casio"));
                        ComPort = port;
                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " Can't find Casio projector on " + port);
                    }
                    //Lets see if the current projector is a Promethean
                    //State what port i am using
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " Promethean Scan - " + port);
                    // Lets ask the currect power state so we are not asking the projector to do something it dosnt need
                    _serialPort.Write(_prometheanprojectorPowerQuery);
                    // Lets wait for the device to respond
                    Thread.Sleep(2);
                    // Put the reply into a string
                    _reply = _serialPort.ReadExisting();
                    if (_reply == _prometheanprojectorResponsePowerOff)
                    {
                        comboBox1.SelectedItem = list.FirstOrDefault(x => x.DisplayName.StartsWith("Pro"));
                        ComPort = port;
                    }
                    if (_reply == _prometheanprojectorResponsePowerOn)
                    {
                        comboBox1.SelectedItem = list.FirstOrDefault(x => x.DisplayName.StartsWith("Pro"));
                        ComPort = port;
                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " Can't find Promethean Projector on " + port);
                    }
                    _serialPort.Close();
                }
                catch (System.IO.IOException)
                {
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " Unable to query port " + port);
                }
            }

            if (string.IsNullOrEmpty(ComPort))
            {

                ComPort = "COM1";
            }

            foreach (Type t in types)
            {
                list.Add(new Projector((IProjectorController)Activator.CreateInstance(t, ComPort)));
            }

            comboBox1.DataSource = list;
            comboBox1.DisplayMember = "DisplayName";

            comboBox1.SelectedItem = list.FirstOrDefault(x => x.DisplayName.StartsWith(Controller));

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            var item = (Projector)comboBox1.SelectedItem;

            _controller = new ControllerDecorator(item.Controller);

            // hook into the message event of the controller and push that into the textbox
            _controller.Message += (o, s) =>
                {
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " " + s);
                };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
       

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            _controller.PowerOff();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _controller.PowerOn();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            _controller.ProjectorSelectRGB();
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            _controller.ProjectorFreeze();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            _controller.ProjectorSelectHDMI();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.ScrollToCaret();
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                try
                {
                    var list = new List<Projector>();
                    comboBox1.DataSource = list;
                    string ComPort = string.Empty;
                    _serialPort = new SerialPort(port);
                    //Lets see if the current projector is a Casio
                    //State what port i am using
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " Casio Scan - " + port);
                    // Open Serial Port
                    _serialPort.Open();
                    // Lets ask the currect power state so we are not asking the projector to do something it dosnt need
                    _serialPort.Write(_casioprojectorPowerQuery);
                    // Lets wait for the device to respond
                    Thread.Sleep(200);
                    // Put the reply into a string
                    _reply = _serialPort.ReadExisting();
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " Recieved " + _reply);
                    if (_reply == _casioprojectorResponsePowerOff)
                    {
                        comboBox1.SelectedItem = list.FirstOrDefault(x => x.DisplayName.StartsWith("Casio"));
                        ComPort = port;
                    }
                    if (_reply == _casioprojectorResponsePowerOn)
                    {
                        comboBox1.SelectedItem = list.FirstOrDefault(x => x.DisplayName.StartsWith("Casio"));
                        ComPort = port;
                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " Can't find Casio projector on " + port);
                    }
                    //Lets see if the current projector is a Promethean
                    //State what port i am using
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " Promethean Scan - " + port);
                    // Lets ask the currect power state so we are not asking the projector to do something it dosnt need
                    _serialPort.Write(_prometheanprojectorPowerQuery);
                    // Lets wait for the device to respond
                    Thread.Sleep(200);
                    // Put the reply into a string
                    _reply = _serialPort.ReadExisting();
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " Recieved " + _reply);
                    if (_reply == _prometheanprojectorResponsePowerOff)
                    {
                        comboBox1.SelectedItem = list.FirstOrDefault(x => x.DisplayName.StartsWith("P"));
                        ComPort = port;
                    }
                    if (_reply == _prometheanprojectorResponsePowerOn)
                    {
                        comboBox1.SelectedItem = list.FirstOrDefault(x => x.DisplayName.StartsWith("P"));
                        ComPort = port;
                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " Can't find Promethean Projector on " + port);
                    }
                    _serialPort.Close();
                }
                catch (System.IO.IOException)
                {
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " Unable to query port " + port);
                }
            }
        }

    }
}
