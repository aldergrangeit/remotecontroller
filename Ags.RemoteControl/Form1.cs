using Ags.ProjectorController;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Collections;

namespace Ags.RemoteControl
{

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
        private int SecertClicks;
        public static DateTime Now { get; set; }

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

            ComPort = "COM1";

            foreach (Type t in types)
            {
                list.Add(new Projector((IProjectorController)Activator.CreateInstance(t, ComPort)));
                Console.WriteLine("{0}\t", t);
            }

            comboBox1.DataSource = list;
            comboBox1.DisplayMember = "DisplayName";

            comboBox1.SelectedItem = list.FirstOrDefault(x => x.DisplayName.StartsWith(Controller));
            Console.Write(Controller + "controller");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
           var item = (Projector)comboBox1.SelectedItem;

            _controller = new ControllerDecorator(item.Controller);

            // hook into the message event of the controller and push that into the textbox
            _controller.Message += (o, s) =>
                {
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " " + s);
                };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            scanforprojectors();
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

        private void button3_Click(object sender, EventArgs e)
        {
            _controller.ProjectorSelectRGB();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _controller.ProjectorFreeze();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            SecertClicks += 1;
            if (SecertClicks == 5)
            {
                Console.WriteLine("Activated");
                secretclicks();
            }
            if (SecertClicks < 5)
            {
                Console.WriteLine("Counting secret clicks as " + SecertClicks);
            }
         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _controller.ProjectorSelectHDMI();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.ScrollToCaret();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            scanforprojectors();
        }

        public void secretclicks()
        {
            
        }

        public void scanforprojectors()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox2.Items.Add(port);
                
                try
                {
                    string ComPort = string.Empty;
                    _serialPort = new SerialPort(port);
                    //Lets see if the current projector is a Casio
                    //State what port i am using
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Casio Scan - " + port);
                    // Open Serial Port
                    _serialPort.BaudRate = 19200;
                    _serialPort.Open();
                    // Lets ask the currect power state so we are not asking the projector to do something it dosnt need
                    _serialPort.Write(_casioprojectorPowerQuery);
                    // Lets wait for the device to respond
                    Thread.Sleep(200);
                    // Put the reply into a string
                    _reply = _serialPort.ReadExisting();
                    if (_reply == "")
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Did not recived a reply ");
                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Recived " + _reply);
                    }
                    if (_reply == _casioprojectorResponsePowerOff)
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Casio Projector Detected on " + port);
                        comboBox1.SelectedItem = _controller;
                        comboBox1.Enabled = false;
                        comboBox2.SelectedItem = port;
                        comboBox2.Enabled = false;
                        _serialPort.Close();
                        break;
                    }
                    if (_reply == _casioprojectorResponsePowerOn)
                    {
                        comboBox1.SelectedItem = _controller;
                        comboBox1.Enabled = false;
                        comboBox2.SelectedItem = ComPort;
                        comboBox2.Enabled = false;
                        _serialPort.Close();
                        break;
                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Can't find Casio projector on " + port);
                    }
                    //Lets see if the current projector is a Promethean
                    //State what port i am using
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Promethean Scan - " + port);
                    // Lets ask the currect power state so we are not asking the projector to do something it dosnt need
                    _serialPort.Write(_prometheanprojectorPowerQuery);
                    // Lets wait for the device to respond
                    Thread.Sleep(200);
                    // Put the reply into a string
                    _reply = _serialPort.ReadExisting();
                    if (_reply == "")
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Did not recived a reply ");
                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Recived " + _reply);
                    }
                    if (_reply == _prometheanprojectorResponsePowerOff)
                    {
                        comboBox1.SelectedItem = "Automatically Detected";
                        comboBox1.Enabled = false;
                        comboBox2.SelectedItem = port;
                        comboBox2.Enabled = false;
                    }
                    if (_reply == _prometheanprojectorResponsePowerOn)
                    {
                        comboBox1.SelectedItem = "Automatically Detected";
                        comboBox1.Enabled = false;
                        comboBox2.SelectedItem = port;
                        comboBox2.Enabled = false;
                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Can't find Promethean Projector on " + port);
                    }
                    _serialPort.Close();
                }
                catch (System.IO.IOException)
                {
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Unable to query port " + port);
                }
            }
        
         }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
