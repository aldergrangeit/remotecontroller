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

namespace Ags.RemoteControl
{
    using Microsoft.Win32;

    public partial class Form1 : Form
    {
        private IProjectorController _controller;

        public Form1()
        {
            InitializeComponent();


            var type = typeof(IProjectorController);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);

            var list = new List<Projector>();

            string ComPort = string.Empty;
            string Controller = string.Empty;

            string[] ports = SerialPort.GetPortNames();

            richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " The following Ports where found");
            Console.WriteLine("The following serial ports were found:");

            foreach (string port in ports)
            {
                try
                {
                    richTextBox1.AppendText(Environment.NewLine + DateTime.Today.TimeOfDay + " " + port);
                    Console.WriteLine(port);
                    _controller.IsMe();
                }
                catch(Win32Exception) {
                Console.WriteLine("Unable to query port");
                }
            }

            if (string.IsNullOrEmpty(ComPort))
            {
                ComPort = "COM1";
            }

            foreach (Type t in types)
            {
                list.Add(new Projector { Controller = (IProjectorController)Activator.CreateInstance(t, ComPort) });
            }

            comboBox1.DataSource = list;
            comboBox1.DisplayMember = "DisplayName";

            comboBox1.SelectedItem = list.FirstOrDefault(x => x.DisplayName.StartsWith(Controller));

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (Projector)comboBox1.SelectedItem;
            _controller = item.Controller;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Lets do some querying to select the correct controller
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

        }

    }

    public class Projector
    {
        
        public string DisplayName
        {
            get
            {
                return string.Format("{0} ({1})", Controller.Make, Controller.Model);
            }
        }

        public IProjectorController Controller { get; set; }
    }
}
