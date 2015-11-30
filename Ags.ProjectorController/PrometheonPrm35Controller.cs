using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;

namespace Ags.ProjectorController
{
    public class PrometheanPrm35Controller : IProjectorController
    {
        private const string _projectorPowerOn = "~PN\r";
        private const string _projectorResponsePowerOn = "On";
        private const string _projectorPowerOff = "~PF\r";
        private const string _projectorResponsePowerOff = "Off";
        private const string _projectorPowerQuery = "~qP\r";
        private const string _projectorSelectRGB = "~SR\r";
        private const string _projectorSelectResponseRGB = "RGB1";
        private const string _projectorSelectResponseNone = "None";
        private const string _projectorSelectResponseHDMI = "HDMI";
        private const string _projectorSelectHDMI = "~SH\r";
        private const string _projectorSelectQuery = "~qS\r";
        private const string _projectorFreeze = "~rF\r";
        private const string _projectorOnReponseSelectFreeze = "On";
        private const string _projectorOffReponseSelectFreeze = "Off";
        private const string _projectorFreezeQuery = "~qZ\r";
        private const string _projectorBlankQuery = "~qK\r";
        private const string _projectorBlankResponseOn = "On";
        private const string _projectorBlankResponseOff = "Off";
        private const string _projectorBlank = "~rB\r";

        private SerialPort _serialPort;

        public PrometheanPrm35Controller(string serialPort)
        {
            _serialPort = new SerialPort(serialPort);
            // Set baud
            _serialPort.BaudRate = 9600;
            Make = "Promethean";
            Model = "PRM-35";

        }

        public event EventHandler<string> Message;

        public string Make { get; set; }

        public string Model { get; set; }
        
        public string _reply = string.Empty;

        public string _replymod = string.Empty;

        public bool job = false;

        public void PowerOn()
        {
            // Open Serial Port
            _serialPort.Open();
            // Lets ask the currect power state so we are not asking the projector to do something it dosnt need
            _serialPort.Write(_projectorPowerQuery);
            // Lets wait for the device to respond
            Thread.Sleep(6000);
            // Put the reply into a string
            _reply = _serialPort.ReadExisting();
            // Due to promethean projectors reply with a little more than they are programmed to, replace this all the extra stuff with nothing
            _replymod = _reply.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("P", "");
            // Are we in an on state already?
            if (_replymod == _projectorResponsePowerOn)
                {
                    // We are already on
                    this.Message.Invoke(this, "Device is already powered on");
                    _serialPort.Close();
                }
            // We are in an off state, lets turn on
            else if (_replymod == _projectorResponsePowerOff)
            {
                    // Write the command required
                    _serialPort.Write(_projectorPowerOn);
                    // Allow a little time for the device to action
                    Thread.Sleep(20000);
                    // Ask the device its current status
                    _serialPort.Write(_projectorPowerQuery);
                    // Allow time for device to respond
                    Thread.Sleep(6000);
                    // Get the response into a string
                    _reply = _serialPort.ReadExisting();
                    // Due to promethean projectors reply with a little more than they are programmed to, replace this all the extra stuff with nothing
                    _replymod = _reply.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("P", "");
                    // Are we now in a On state and if not lets start to feedback to user for futher investigation
                    if (_replymod == _projectorResponsePowerOn)
                    {
                        // All done, ITS ALIVE!
                        this.Message.Invoke(this,"Device has powered on and is ready for use");
                        _serialPort.Close();
                    }
                    else
                    {
                        // Lets assume the device is not connected anymore or not responding
                        this.Message.Invoke(this, "The device is not responding");
                        _serialPort.Close();
                    }
            }
            // Lets assume the device is not connected anymore or not responding
            else
            {
                    this.Message.Invoke(this,"The device is not responding");
                    _serialPort.Close();
            }
        }

        public void PowerOff()
        {
            // Open Serial Port
            _serialPort.Open();
            // Lets ask the currect power state so we are not asking the projector to do something it dosnt need
            _serialPort.Write(_projectorPowerQuery);
            // Lets wait for the device to respond
            Thread.Sleep(2000);
            // Put the reply into a string
            _reply = _serialPort.ReadExisting();
            // Due to promethean projectors reply with a little more than they are programmed to, replace this all the extra stuff with nothing
            _replymod = _reply.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("P", "");
            // Are we in an on state
                if (_replymod == _projectorResponsePowerOff)
                {
                    // We are already off
                    this.Message.Invoke(this, "Device is already powered off");
                    _serialPort.Close();
                }
                // We are in a on state, lets turn off
                else if (_replymod == _projectorResponsePowerOn)
                {
                    // Write the command required
                    _serialPort.Write(_projectorPowerOff);
                    // Allow a little time for the device to action
                    Thread.Sleep(6000);
                    // Ask the device its current status
                    _serialPort.Write(_projectorPowerQuery);
                    // Lets wait for the projector to respond
                    Thread.Sleep(6000);
                    // Put the response into a string
                    _reply = _serialPort.ReadExisting();
                    // Due to promethean projectors reply with a little more than they are programmed to, replace this all the extra stuff with nothing
                    _replymod = _reply.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("P", "");
                        // Are we now in a On state
                        if (_replymod == _projectorResponsePowerOff)
                        {
                            // Device is now off WOOP
                            this.Message.Invoke(this,"Device has powered off");
                            _serialPort.Close();
                        }
                        else
                        {
                            // Lets assume the device is not connected anymore or not responding
                            this.Message.Invoke(this,"The device is not responding");
                            _serialPort.Close();
                        }
                }
                // Lets assume the device is not connected anymore or not responding
                else
                {
                    this.Message.Invoke(this,"The device is not responding");
                    _serialPort.Close();
                }
        }

        public void ProjectorSelectRGB()
        {
            // Open Serial Port
            _serialPort.Open();
            // Lets ask the currect power state so we are not asking the projector to do something it dosnt need
            _serialPort.Write(_projectorSelectQuery);
            // Lets wait for the device to respond
            Thread.Sleep(6000);
            // Put the reply into a string
            _reply = _serialPort.ReadExisting();
            // Due to promethean projectors reply with a little more than they are programmed to, replace this all the extra stuff with nothing
            _replymod = _reply.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("P", "");
            // Are we in an on state
            if (_replymod == _projectorSelectResponseRGB)
            {
                this.Message.Invoke(this, "The device is already on RGB");
                _serialPort.Close();
            }
            else if (_replymod == _projectorSelectResponseHDMI | _replymod == _projectorSelectResponseNone)
            {
                // Write the command required
                _serialPort.Write(_projectorSelectRGB);
                // Allow a little time for the device to action
                Thread.Sleep(6000);
                // Ask the device its current status
                _serialPort.Write(_projectorSelectQuery);
                // Put the reply into a string
                _reply = _serialPort.ReadExisting();
                // Due to promethean projectors reply with a little more than they are programmed to, replace this all the extra stuff with nothing
                _replymod = _reply.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("P", "");
                // Are we now in a On state and if not lets start to feedback to user for futher investigation
                if (_replymod == _projectorSelectResponseRGB)
                {
                    // Device has selected the correct source
                    this.Message.Invoke(this,"Device has changed its source to RGB");
                    _serialPort.Close();
                }
                
                else
                {
                    // Lets assume the device is not connected anymore or not responding
                    this.Message.Invoke(this,"The device is not responding");
                    _serialPort.Close();
                }
            }
            // Lets assume the device is not connected anymore or not responding
            else
            {
                this.Message.Invoke(this,"The device is not responding");
                _serialPort.Close();
            }
        }

        public void ProjectorSelectHDMI()
        {
            // Open Serial Port
            _serialPort.Open();
            // Lets ask the correct power state so we are not asking the projector to do something it dosnt need
            _serialPort.Write(_projectorSelectQuery);
            // Lets wait for the device to respond
            Thread.Sleep(6000);
            // Put the reply into a string
            _reply = _serialPort.ReadExisting();
            // Due to promethean projectors reply with a little more than they are programmed to, replace this all the extra stuff with nothing
            _replymod = _reply.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("P", "");
            // Are we in an on state
            if (_replymod == _projectorSelectResponseRGB | _replymod == _projectorSelectResponseNone)
            {
                // Write the command required
                _serialPort.Write(_projectorSelectHDMI);
                // Allow a little time for the device to action
                Thread.Sleep(6000);
                // Ask the device its current status
                _serialPort.Write(_projectorSelectQuery);
                // Let the device respond
                Thread.Sleep(6000);
                // Put the respond into a string
                _reply = _serialPort.ReadExisting();
                // Due to promethean projectors reply with a little more than they are programmed to, replace this all the extra stuff with nothing
                _replymod = _reply.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("P", "");
                // We are already in HDMI
                if (_replymod == _projectorSelectResponseHDMI)
                {
                    this.Message.Invoke(this, "Device has has changed its source to HDMI");
                    _serialPort.Close();
                }
                // Are we now in a On state and if not lets start to feedback to user for futher investigation
                    if (_replymod == _projectorSelectResponseHDMI)
                    {
                        this.Message.Invoke(this,"Device has has changed its source to HDMI");
                        _serialPort.Close();
                    }
                // Lets assume the device is not connected anymore or not responding
                    if (_replymod == _projectorSelectResponseNone)
                    {
                        this.Message.Invoke(this, "Unable to change input, this is due to the source being unavailable");
                        _serialPort.Close();
                    }
                    else
                    {
                        this.Message.Invoke(this,"The device is not responding");
                        _serialPort.Close();
                    }
            }
            // Lets assume the device is not connected anymore or not responding
            else
            {
                this.Message.Invoke(this, "The device is not responding 1");
                _serialPort.Close();
            }
        }

        public void ProjectorFreeze()
        {
            {
                // Open serial port
                _serialPort.Open();
                // Query projector for current status
                _serialPort.Write(_projectorFreezeQuery);
                // Sleep to allow the projector to react
                Thread.Sleep(2000);
                // Write the reply into a string
                _reply = _serialPort.ReadExisting();
                // Due to promethean projectors reply with a little more than they are programmed to, replace this all the extra stuff with nothing
                _replymod = _reply.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("P", "");
                // Make sure its not a null result
                if (_replymod != null)
                {
                    job = false;
                    // We are not froze
                    if (_replymod == _projectorOffReponseSelectFreeze & job == false)
                    {
                        // Send the blnk command
                        _serialPort.Write(_projectorFreeze);
                        // Let the projector react
                        Thread.Sleep(1000);
                        // Query the projector
                        _serialPort.Write(_projectorFreezeQuery);
                        // Place repy into string
                        Thread.Sleep(1000);
                        _reply = _serialPort.ReadExisting();
                        // Due to promethean projectors reply with a little more than they are programmed to, replace this all the extra stuff with nothing
                        _replymod = _reply.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("P", "");
                        if (_replymod == _projectorOnReponseSelectFreeze)
                        {
                            this.Message.Invoke(this, "The device is froze");
                            job = true;
                        }

                    }
                    // We are blanked
                    if (_replymod == _projectorOnReponseSelectFreeze & job == false)
                    {
                        // Send the blnk command
                        _serialPort.Write(_projectorFreeze);
                        // Query the projector
                        // Let the projector react
                        Thread.Sleep(1000);
                        _serialPort.Write(_projectorFreezeQuery);
                        // Let the projector react
                        Thread.Sleep(1000);
                        // Place reply into string
                        _reply = _serialPort.ReadExisting();
                        // Due to promethean projectors reply with a little more than they are programmed to, replace this all the extra stuff with nothing
                        _replymod = _reply.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("P", "");
                        if (_replymod == _projectorOffReponseSelectFreeze)
                        {
                            this.Message.Invoke(this, "The device is free");
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

        public void ProjectorBlank()
        {
            {
                // Open serial port
                _serialPort.Open();
                // Query projector for current status
                _serialPort.Write(_projectorBlankQuery);
                // Sleep to allow the projector to react
                Thread.Sleep(1000);
                // Write the reply into a string
                _reply = _serialPort.ReadExisting();
                // Due to promethean projectors reply with a little more than they are programmed to, replace this all the extra stuff with nothing
                _replymod = _reply.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("P", "");
                // Make sure its not a null result
                if (_replymod != null)
                {
                    job = false;
                    // We are blanked already
                    if (_replymod == _projectorBlankResponseOn & job == false)
                    {
                        // Send the blnk command
                        _serialPort.Write(_projectorBlank);
                        // Let the projector react
                        Thread.Sleep(1000);
                        // Query the projector
                        _serialPort.Write(_projectorBlankQuery);
                        // Place reply into string
                        Thread.Sleep(1000);
                        _reply = _serialPort.ReadExisting();
                        // Due to promethean projectors reply with a little more than they are programmed to, replace this all the extra stuff with nothing
                        _replymod = _reply.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("P", "");
                        if (_replymod == _projectorBlankResponseOff)
                        {
                            this.Message.Invoke(this, "The device is unblanked");
                            job = true;
                        }

                    }
                    // We are not blanked
                    if (_replymod == _projectorBlankResponseOff & job == false)
                    {
                        // Send the blnk command
                        _serialPort.Write(_projectorBlank);
                        // Query the projector
                        // Let the projector react
                        Thread.Sleep(1000);
                        _serialPort.Write(_projectorBlankQuery);
                        // Let the projector react
                        Thread.Sleep(1000);
                        // Place reply into string
                        _reply = _serialPort.ReadExisting();
                        // Due to promethean projectors reply with a little more than they are programmed to, replace this all the extra stuff with nothing
                        _replymod = _reply.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("P", "");
                        if (_replymod == _projectorBlankResponseOn)
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
}
