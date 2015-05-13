using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ags.ProjectorController;
using System.Reflection;

namespace Ags.RemoteControl
{
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

            var list = new List<Type>();

            foreach (Type t in types)
            {
                list.Add(t);
            }

            comboBox1.DataSource = list;
            comboBox1.DisplayMember = "Name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _controller.PowerOn();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _controller.PowerOff();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "PrometheonPrm35Controller":
                    _controller = new PrometheonPrm35Controller("COM1");
                    break;
                case "CasioController":
                    _controller = new CasioController();
                    break;
                default:
                    break;
            }
        }
    }
}
