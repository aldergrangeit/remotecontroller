using System;
using System.IO.Ports;
using System.Threading;

namespace Ags.ProjectorController
{
    public class CasioController : IProjectorController
    {

        private const string _projectorPowerOn = "(PWR1)";
        private const string _projectorResponsePowerOn = "(0-1,1)";
        private const string _projectorPowerOff = "(PWR0)";
        private const string _projectorResponsePowerOff = "(0-1,0)";
        private const string _projectorPowerQuery = "(PWR?)";
        private const string _projectorSelectRGB = "(SRC0)";
        private const string _projectorSelectResponseRGB = "(0-10,0)";
        private const string _projectorSelectResponseHDMI = "(0-10,7)";
        private const string _projectorSelectHDMI = "(SRC7)";
        private const string _projectorSelectQuery = "(SRC?)";
        private const string _projectorOnBlank = "(BLK1)";
        private const string _projectorOffBlank = "(BLK0)";
        private const string _projectorBlankOnReponse = "(0-1,1)";
        private const string _projectorBlankOffReponse = "(0-1,0)";
        private const string _projectorBlankQuery = "(BLK?)";

        private SerialPort _serialPort;
         
        public CasioController(string serialPort)
        {
            _serialPort = new SerialPort(serialPort);
            // Set baud
            _serialPort.BaudRate = 19200;
            Make = "Casio";
            Model = "All";
        }

        public event EventHandler<string> Message;

        public string Make { get; set; }

        public string Model { get; set; }

        public string _reply = string.Empty;

        public bool job = false;

        public void PowerOn()
        {
            // Open Serial Port
            _serialPort.Open();
            _serialPort.DiscardInBuffer();
            _serialPort.DiscardOutBuffer();
            // Lets ask the currect power state so we are not asking the projector to do something it dosnt need
            _serialPort.Write(_projectorPowerQuery);
            // Lets wait for the device to respond
            Thread.Sleep(2000);
            // Put the reply into a string
            _reply = _serialPort.ReadExisting();
            // Are we in an off state
            if (_reply == _projectorResponsePowerOn)
            {
                this.Message.Invoke(this, "Device is already powered on");
                _serialPort.Close();
            }
            if (_reply == _projectorResponsePowerOff)
            {
                // Write the command required
                _serialPort.Write(_projectorPowerOn);
                // Allow a little time for the device to action
                Thread.Sleep(3000);
                // Ask the device its current status
                _serialPort.Write(_projectorPowerQuery);
                Thread.Sleep(2000);
                _reply = _serialPort.ReadExisting();
                // Are we now in a On state and if not lets start to feedback to user for futher investigation
                if (_reply == _projectorResponsePowerOn)
                {
                    //TODO Allow user feedback to richtext box in form1
                    this.Message.Invoke(this,"Device has powered on and is ready for use");
                    _serialPort.Close();
                }
                // Lets assume the device is not connected anymore or not responding
                else
                {
                    this.Message.Invoke(this,"The device is not responding");
                    _serialPort.Close();
                }
            }
            // Lets assume the device is not connected anymore or not responding
            else
            {
                _serialPort.Close();
            }
        }

        public void PowerOff()
        {
            // Open Serial Port
            _serialPort.Open();
            _serialPort.DiscardInBuffer();
            _serialPort.DiscardOutBuffer();
            // Lets ask the currect power state so we are not asking the projector to do something it dosnt need
            _serialPort.Write(_projectorPowerQuery);
            // Lets wait for the device to respond
            Thread.Sleep(2000);
            // Put the reply into a string
            _reply = _serialPort.ReadExisting();
            if (_reply == _projectorResponsePowerOff)
            {
                //TODO Allow user feedback to richtext box in form1
                this.Message.Invoke(this, "The device is already off");
                _serialPort.Close();
            }
            // Are we in an on state
            if (_reply == _projectorResponsePowerOn)
            {
                // Write the command required
                _serialPort.Write(_projectorPowerOff);
                // Allow a little time for the device to action
                Thread.Sleep(4000);
                // Ask the device its current status
                _serialPort.Write(_projectorPowerQuery);
                Thread.Sleep(4000);
                _reply = _serialPort.ReadExisting();
                // Are we now in a On state and if not lets start to feedback to user for futher investigation
                if (_reply == _projectorResponsePowerOff)
                {
                    //TODO Allow user feedback to richtext box in form1
                    this.Message.Invoke(this,"Device has powered off");
                    _serialPort.Close();
                }
                // Lets assume the device is not connected anymore or not responding
                else
                {
                    this.Message.Invoke(this,"The device is not responding");
                    _serialPort.Close();
                }
            }
            // Lets assume the device is not connected anymore or not responding
            else
            {
                _serialPort.Close();
            }
        }

        public void ProjectorSelectRGB()
        {
            // Open Serial Port
            _serialPort.Open();
            // Clear out buffer just in case we have anything in there we dont want
            _serialPort.DiscardInBuffer();
            _serialPort.DiscardOutBuffer();
            // Lets ask the currect input source so we are not asking the projector to do something it dosnt need
            _serialPort.Write(_projectorSelectQuery);
            // Lets wait for the device to respond
            Thread.Sleep(2000);
            // Put the reply into a string
            _reply = _serialPort.ReadExisting();
            // We are already in VGA
            if (_reply != null)
            {
                if (_reply == _projectorSelectResponseRGB)
                {
                    this.Message.Invoke(this, "The projector is already on RGB as it's input");
                    _serialPort.Close();
                }
                // Are we are are in HDMI
                if (_reply == _projectorSelectResponseHDMI)
                {
                    // Write the command required
                    _serialPort.Write(_projectorSelectRGB);
                    // Allow a little time for the device to action
                    Thread.Sleep(4000);
                    // Ask the device its current status
                    _serialPort.Write(_projectorSelectQuery);
                    Thread.Sleep(6000);
                    _reply = _serialPort.ReadExisting();
                    // Are we now in VGA mode
                    if (_reply == _projectorSelectResponseRGB)
                    {
                        //TODO Allow user feedback to richtext box in form1
                        this.Message.Invoke(this, "The projector is now in RGB mode");
                        _serialPort.Close();
                    }
                }
            }
            else
            {
                // Lets assume the device is not connected anymore or not responding
                this.Message.Invoke(this, "The device is not responding");
                _serialPort.Close();
            }
        }

        public void ProjectorSelectHDMI()
        {
            // Open Serial Port
            _serialPort.Open();
            // Clear out buffer just in case we have anything in there we dont want
            _serialPort.DiscardInBuffer();
            _serialPort.DiscardOutBuffer();
            // Lets ask the currect input source so we are not asking the projector to do something it dosnt need
            _serialPort.Write(_projectorSelectQuery);
            // Lets wait for the device to respond
            Thread.Sleep(2000);
            // Put the reply into a string
            _reply = _serialPort.ReadExisting();
            //Have we received a reply form the projector
            if (_reply != null)
            {
                // We are already in HDMI
                if (_reply == _projectorSelectResponseHDMI)
                {
                    this.Message.Invoke(this, "The projector is already on HDMI as the input");
                    _serialPort.Close();
                }
                // Are we are are in HDMI
                if (_reply == _projectorSelectResponseRGB)
                {
                    // Write the command required
                    _serialPort.Write(_projectorSelectHDMI);
                    // Allow a little time for the device to action
                    Thread.Sleep(3000);
                    // Ask the device its current status
                    _serialPort.Write(_projectorSelectQuery);
                    Thread.Sleep(2000);
                    _reply = _serialPort.ReadExisting();
                    // Are we now in HDMI mode
                    if (_reply == _projectorSelectResponseHDMI)
                    {
                        //TODO Allow user feedback to richtext box in form1
                        this.Message.Invoke(this, "The projector is now in HDMI mode");
                        _serialPort.Close();
                    }
                }
            }
            else
            {
                // Lets assume the device is not connected anymore or not responding
                this.Message.Invoke(this, "The device is not responding");
                _serialPort.Close();
            }
        }

        public void ProjectorFreeze()
        {

        }

        public void ProjectorBlank()
        {
            // Open serial port
            _serialPort.Open();
            // Flush serial buffer
            _serialPort.DiscardInBuffer();
            _serialPort.DiscardOutBuffer();
            // Query projector for current status
            _serialPort.Write(_projectorBlankQuery);
            // Sleep to allow the projector to react
            Thread.Sleep(2000);
            // Write the reply into a string
            _reply = _serialPort.ReadExisting();
            
            if (_reply != null)
            {
                job = false;
                // We are blanked already
                if (_reply == _projectorBlankOnReponse & job == false)
                {
                    // Send the blnk command
                    _serialPort.Write(_projectorOffBlank);
                    // Let the projector react
                    Thread.Sleep(2000);
                    // Query the projector
                    _serialPort.Write(_projectorBlankQuery);
                    // Place repy into string
                    Thread.Sleep(2000);
                    _reply = _serialPort.ReadExisting();
                    if (_reply == _projectorBlankOffReponse)
                    {
                        this.Message.Invoke(this, "The device is unblanked");
                        job = true;
                    }

                }
                // We are not blanked
                if (_reply == _projectorBlankOffReponse & job == false)
                {
                    // Send the blnk command
                    _serialPort.Write(_projectorOnBlank);
                    // Query the projector
                    // Let the projector react
                    Thread.Sleep(2000);
                    _serialPort.Write(_projectorBlankQuery);
                    // Place reply into string
                    // Let the projector react
                    Thread.Sleep(2000);
                    _reply = _serialPort.ReadExisting();
                    if (_reply == _projectorBlankOnReponse)
                    {
                        this.Message.Invoke(this, "The device is blanked");
                        job = true;
                    }
                }
                _serialPort.Close();
            }
            else
            {
                // Lets assume the device is not connected anymore or not responding
                this.Message.Invoke(this, "The device is not responding");
                _serialPort.Close();
            }

        }
    }
}
