using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Ags.ProjectorController
{
    public class PrometheonPrm35Controller : IProjectorController
    {
        private const string _projectorPowerOn = "~PN\r";
        private const string _projectorPowerOff = "~PF\r";
        private const string _projectorAutoImage = "~AI\r";
        private const string _projectorSelectRGB = "~SR\r";
        private const string _projectorSelectRGB2 = "~SG\r";
        private const string _projectorSelectVideo = "~SV\r";
        private const string _projectorSelectSVideo = "~SS\r";
        private const string _projectorSelectComponent = "~SY\r";
        private const string _projectorSelectHDMI = "~SH\r";

        private SerialPort _serialPort;

        public PrometheonPrm35Controller(string serialPort)
        {
            _serialPort = new SerialPort(serialPort);
            
        }

        public void PowerOn()
        {
            _serialPort.Open();
            _serialPort.Write(_projectorPowerOn);
            _serialPort.Close();
            Console.Write("I have Sent the Power On Command");
        }

        public void PowerOff()
        {
            _serialPort.Open();
            _serialPort.Write(_projectorPowerOff);
            _serialPort.Close();
            Console.Write("I have Sent the Power Off Command");
        }

        public void ProjectorAuto()
        {
            _serialPort.Open();
            _serialPort.Write(_projectorAutoImage);
            _serialPort.Close();
            Console.Write("I have Sent the Auto Image Command");
        }

        public void ProjectorSelectRGB()
        {
            _serialPort.Open();
            _serialPort.Write(_projectorSelectRGB);
            _serialPort.Close();
            Console.Write("I have Sent the RGB Input 1 Command");
        }

        public void ProjectorSelectRGB2()
        {
            _serialPort.Open();
            _serialPort.Write(_projectorSelectRGB2);
            _serialPort.Close();
            Console.Write("I have Sent the RGB Input 2 Command");
        }

        public void ProjectorSelectVideo()
        {
            _serialPort.Open();
            _serialPort.Write(_projectorSelectVideo);
            _serialPort.Close();
            Console.Write("I have Sent the Video Command");
        }

        public void ProjectorSelectSVideo()
        {
            _serialPort.Open();
            _serialPort.Write(_projectorSelectSVideo);
            _serialPort.Close();
            Console.Write("I have Sent the SVideo Command");
        }

        public void ProjectorSelectComponent()
        {
            _serialPort.Open();
            _serialPort.Write(_projectorSelectComponent);
            _serialPort.Close();
            Console.Write("I have Sent the Component Command");
        }

        public void ProjectorSelectHDMI()
        {
            _serialPort.Open();
            _serialPort.Write(_projectorSelectHDMI);
            _serialPort.Close();
            Console.Write("I have Sent the HDMI Command");
        }

        public void BlahBlahFunction()
        {
            throw new NotImplementedException("This projector doesnt know how to blah");
        }
    }
}
