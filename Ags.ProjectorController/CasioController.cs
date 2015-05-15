﻿using System;
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
        private const string _projectorSelectResponseRGB = "(1-7,0)";
        private const string _projectorSelectResponseHDMI = "(1-7,7)";
        private const string _projectorSelectHDMI = "(SRC7)";
        private const string _projectorSelectQuery = "(SRC?)";
        private const string _projectorFreeze = "(BLK1)";
        private const string _projectorOnReponseSelectFreeze = "(0-1,1)";
        private const string _projectorOffReponseSelectFreeze = "(0-1,0)";
        private const string _projectorFreezeQuery = "(BLK?)";

        private SerialPort _serialPort;

        public CasioController(string serialPort)
        {
            _serialPort = new SerialPort(serialPort);
            Make = "Casio";
            Model = "All";
        }

        public event EventHandler<string> Message;

        public string Make { get; set; }

        public string Model { get; set; }

        public bool IsMeBool = false;

        public string _reply = string.Empty;

        public void PowerOn()
        {
            //State what port i am using
            this.Message.Invoke(this, _serialPort.PortName);
            // Open Serial Port
            _serialPort.Open();
            // Lets ask the currect power state so we are not asking the projector to do something it dosnt need
            _serialPort.Write(_projectorPowerQuery);
            // Lets wait for the device to respond
            Thread.Sleep(6000);
            // Put the reply into a string
            _reply = _serialPort.ReadExisting();
            // Are we in an off state
            if (_reply == _projectorResponsePowerOff)
            {
                // Write the command required
                _serialPort.Write(_projectorPowerOn);
                // Output some progess data to console
                this.Message.Invoke(this,"I have Sent " + _projectorPowerOn + " to the device, and awaiting " + _projectorResponsePowerOn + " as a response");
                // Allow a little time for the device to action
                Thread.Sleep(6000);
                // Ask the device its current status
                _serialPort.Write(_projectorPowerQuery);
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
            this.Message.Invoke(this,"Query the device for its current state");
            // Lets wait for the device to respond
            Thread.Sleep(6000);
            // Put the reply into a string
            _reply = _serialPort.ReadExisting();
            // Are we in an on state
            if (_reply == _projectorResponsePowerOn)
            {
                // Write the command required
                _serialPort.Write(_projectorPowerOff);
                // Output some progess data to console
                this.Message.Invoke(this,"I have Sent " + _projectorPowerOff + " to the device, and awaiting " + _projectorResponsePowerOff + " as a response");
                // Allow a little time for the device to action
                Thread.Sleep(6000);
                // Ask the device its current status
                _serialPort.Write(_projectorPowerQuery);
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
            this.Message.Invoke(this,"Query the device for its current state");
            // Lets wait for the device to respond
            Thread.Sleep(6000);
            // Put the reply into a string
            _reply = _serialPort.ReadExisting();
            // Are we in an on state
            if (_reply == _projectorSelectResponseHDMI)
            {
                // Write the command required
                _serialPort.Write(_projectorSelectRGB);
                // Output some progess data to console
                this.Message.Invoke(this,"I have Sent " + _projectorSelectRGB + " to the device, and awaiting " + _projectorSelectResponseRGB + " as a response");
                // Allow a little time for the device to action
                Thread.Sleep(6000);
                // Ask the device its current status
                _serialPort.Write(_projectorSelectQuery);
                _reply = _serialPort.ReadExisting();
                // Are we now in a On state and if not lets start to feedback to user for futher investigation
                if (_reply == _projectorSelectResponseRGB)
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
                this.Message.Invoke(this,"The device is not responding");
                _serialPort.Close();
            }
        }

        public void ProjectorSelectHDMI()
        {
            // Open Serial Port
            _serialPort.Open();
            // Lets ask the currect power state so we are not asking the projector to do something it dosnt need
            _serialPort.Write(_projectorSelectQuery);
            this.Message.Invoke(this,"Query the device for its current state");
            // Lets wait for the device to respond
            Thread.Sleep(6000);
            // Put the reply into a string
            _reply = _serialPort.ReadExisting();
            // Are we in an on state
            if (_reply == _projectorSelectResponseRGB)
            {
                // Write the command required
                _serialPort.Write(_projectorSelectHDMI);
                // Output some progess data to console
                this.Message.Invoke(this,"I have Sent " + _projectorSelectHDMI + " to the device, and awaiting " + _projectorSelectResponseHDMI + " as a response");
                // Allow a little time for the device to action
                Thread.Sleep(6000);
                // Ask the device its current status
                _serialPort.Write(_projectorSelectQuery);
                _reply = _serialPort.ReadExisting();
                // Are we now in a On state and if not lets start to feedback to user for futher investigation
                if (_reply == _projectorSelectResponseHDMI)
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
        }

        public void ProjectorFreeze()
        {
            // Open Serial Port
            _serialPort.Open();
            // Send projector freeze
            _serialPort.Write(_projectorFreeze);
            // Close the serial port
            _serialPort.Close();
        }
    }
}
