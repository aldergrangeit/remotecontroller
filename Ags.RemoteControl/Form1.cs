using Ags.ProjectorController;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Reflection;
using System.Diagnostics;

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
        private const string _casioprojectorLampHoursQuery = "(LMP?)";
        private const string _prometheanprojectorPowerOn = "~PN\r";
        private const string _prometheanprojectorResponsePowerOn = "On\r";
        private const string _prometheanprojectorPowerOff = "~PF\r";
        private const string _prometheanprojectorResponsePowerOff = "Off\r";
        private const string _prometheanprojectorPowerQuery = "~qP\r";
        private const string _prometheanprojectorLampHoursQuery = "~qL\r";
        private const string eventsource = "AG - Virtual Remote";
        private const string eventsourceoperation = "Operation";
        private string selecteditem = string.Empty;
        private string _reply = string.Empty;
        private int SecertClicks;
        public static DateTime Now { get; set; }

        
        public Form1()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
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

            selecteditem = comboBox1.Text.ToString();

            if (selecteditem == "Promethean (PRM-35)")
            {
                button7.Enabled = true;
                button6.Enabled = true;
                button5.Enabled = true;
                button4.Enabled = true;
                button3.Enabled = true;
                button2.Enabled = true;
                button1.Enabled = true;
            }
            else if (selecteditem == "Casio (All)")
            {
                button7.Enabled = true;
                button6.Enabled = true;
                button5.Enabled = false;
                button4.Enabled = true;
                button3.Enabled = true;
                button2.Enabled = true;
                button1.Enabled = true;
            }
            else
            {
                richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Please select a the projector");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            scanforprojectors();
        }

        private void Form1_Closed(object sender, EventArgs e)
        {
            _serialPort.Close();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void buttonsdisable()
        {

           button7.Enabled = false;
           button6.Enabled = false;
           button5.Enabled = false;
           button4.Enabled = false;
           button3.Enabled = false;
           button2.Enabled = false;
           button1.Enabled = false;
        }

        private void buttonsenable()
        {
            if (selecteditem == "Promethean (PRM-35)")
            {
                button7.Enabled = true;
                button6.Enabled = true;
                button5.Enabled = true;
                button4.Enabled = true;
                button3.Enabled = true;
                button2.Enabled = true;
                button1.Enabled = true;
            }
            else if (selecteditem == "Casio (All)")
            {
                button7.Enabled = true;
                button6.Enabled = true;
                button5.Enabled = false;
                button4.Enabled = true;
                button3.Enabled = true;
                button2.Enabled = true;
                button1.Enabled = true;
            }
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            buttonsdisable();
            _controller.PowerOff();
            buttonsenable();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonsdisable();
            _controller.PowerOn();
            buttonsenable();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            buttonsdisable();
            _controller.ProjectorSelectRGB();
            buttonsenable();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            buttonsdisable();
            _controller.ProjectorFreeze();
            buttonsenable();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            SecertClicks += 1;
            if (SecertClicks == 5)
            {
                richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Technician Mode Activated");
                secretclicks();
            }
            if (SecertClicks < 5)
            {
                Console.WriteLine("Counting secret clicks as " + SecertClicks);
            }
         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            buttonsdisable();
            _controller.ProjectorSelectHDMI();
            buttonsenable();
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
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            button6.Enabled = true;
            button5.Enabled = true;
            button4.Enabled = true;
            button3.Enabled = true;
            button2.Enabled = true;
            button1.Enabled = true;

            richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Current Lamp Hours - " + _reply);
        }

        public void scanforprojectors()
        {
            if (!EventLog.SourceExists("ProjectorSearchService"))
            {
                //An event log source should not be created and immediately used.
                //There is a latency time to enable the source, it should be created
                //prior to executing the application that uses the source.
                //Execute this sample a second time to use the new source.
                EventLog.CreateEventSource("ProjectorSearchService", "Virtual Remote");
                // The source is created.  Exit the application to allow it to be registered.
                return;
            }

            // Create an EventLog instance and assign its source.
            EventLog myLog = new EventLog();
            myLog.Source = "ProjectorSearchService";

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
                    if (_reply == null)
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Did not recived a reply ");
                    }
                    if (_reply == _casioprojectorResponsePowerOff)
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Casio Projector Detected on " + port);
                        comboBox1.SelectedIndex = comboBox1.FindStringExact("Casio (All)");
                        comboBox1.Enabled = false;
                        comboBox2.SelectedItem = port;
                        comboBox2.Enabled = false;
                        button4.Enabled = true;
                        button3.Enabled = true;
                        button2.Enabled = true;
                        button1.Enabled = true;
                        _serialPort.Write(_casioprojectorLampHoursQuery);
                        Thread.Sleep(1000);
                        _reply = _serialPort.ReadExisting();
                        _serialPort.DiscardOutBuffer();
                        _serialPort.Close();
                        myLog.WriteEntry("Current Hours Run - " + _reply + "Projector dectected - Casio" + "\r" + "Connected to port - " + port);
                        break;
                    }
                    if (_reply == _casioprojectorResponsePowerOn)
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Casio Projector Detected on " + port);
                        comboBox1.SelectedIndex = comboBox1.FindStringExact("Casio (All)");
                        comboBox1.Enabled = false;
                        comboBox2.SelectedItem = port;
                        comboBox2.Enabled = false;
                        button4.Enabled = true;
                        button3.Enabled = true;
                        button2.Enabled = true;
                        button1.Enabled = true;
                        _serialPort.Write(_casioprojectorLampHoursQuery);
                        Thread.Sleep(1000);
                        _reply = _serialPort.ReadExisting();
                        _serialPort.DiscardOutBuffer();
                        _serialPort.Close();
                        myLog.WriteEntry("Current Hours Run - " + _reply + "Projector dectected - Casio" + "\r" + "Connected to port - " + port);
                        break;
                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Can't find Casio projector on " + port);
                        _serialPort.Close();
                        myLog.WriteEntry("Unable to find a Casio projector");
                        
                    }
                    //Lets see if the current projector is a promethean
                    //State what port i am using
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Promethean Scan - " + port);
                    // Open Serial Port
                    _serialPort.BaudRate = 9600;
                    _serialPort.Open();
                    // Lets ask the currect power state so we are not asking the projector to do something it dosnt need
                    _serialPort.Write(_prometheanprojectorPowerQuery);
                    Thread.Sleep(200);
                    _serialPort.DiscardOutBuffer();
                    _serialPort.DiscardInBuffer();
                    _serialPort.Write(_prometheanprojectorPowerQuery);
                    // Lets wait for the device to respond
                    Thread.Sleep(200);
                    // Put the reply into a string
                    _reply = _serialPort.ReadExisting();
                    if (_reply != null)
                    {
                        if (_reply == _prometheanprojectorResponsePowerOff)
                        {
                            richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Promethean Projector Detected on " + port);
                            comboBox1.SelectedIndex = comboBox1.FindStringExact("Promethean (PRM-35)");
                            comboBox1.Enabled = false;
                            comboBox2.SelectedItem = port;
                            comboBox2.Enabled = false;
                            button5.Enabled = true;
                            button4.Enabled = true;
                            button3.Enabled = true;
                            button2.Enabled = true;
                            button1.Enabled = true;
                            _serialPort.Write(_prometheanprojectorLampHoursQuery);
                            Thread.Sleep(1000);
                            _reply = _serialPort.ReadExisting();
                            _serialPort.DiscardOutBuffer();
                            _serialPort.Close();
                            myLog.WriteEntry("Current Hours Run - " + _reply + "Projector dectected - Casio" + "\r" + "Connected to port - " + port);
                            break;
                        }
                        if (_reply == _prometheanprojectorResponsePowerOn)
                        {
                            richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Promethean Projector Detected on " + port);
                            comboBox1.SelectedIndex = comboBox1.FindStringExact("Promethean (PRM-35)");
                            comboBox1.Enabled = false;
                            comboBox2.SelectedItem = port;
                            comboBox2.Enabled = false;
                            button5.Enabled = true;
                            button4.Enabled = true;
                            button3.Enabled = true;
                            button2.Enabled = true;
                            button1.Enabled = true;
                            _serialPort.Write(_prometheanprojectorLampHoursQuery);
                            Thread.Sleep(1000);
                            _reply = _serialPort.ReadExisting();
                            _serialPort.DiscardOutBuffer();
                            _serialPort.Close();
                            myLog.WriteEntry("Current Hours Run - " + _reply + "Projector dectected - Casio" + "\r" + "Connected to port - " + port);
                            break;
                        }
                        else
                        {
                            richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Can't find Promethean Projector on " + port);
                            myLog.WriteEntry("Unable to find a Promethean projector");
                        }
                    }
                    else
                    {
                        richTextBox1.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " Received " + _reply);
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
            _controller.ProjectorBlank();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://ithelpdesk.aldergrange.com/portal");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://ithelpdesk.aldergrange.com/portal");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a new instance of the Form2 class
            Form2 aboutForm = new Form2();

            // Show the settings form
            aboutForm.Show();
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (CloseCancel() == false)
            {
                e.Cancel = true;
            };
        }

        public static bool CloseCancel()
        {
            const string message = "Are you sure that you would like to close the virtual remote?";
            const string caption = "Close remote";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                return true;
            else
                return false;
        }
    }
}
