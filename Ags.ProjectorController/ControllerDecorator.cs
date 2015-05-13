namespace Ags.ProjectorController
{
    using System;

    /// <summary>
    /// This decorator is used to provide a wrapper around a controller to give global functionality around
    /// each method in the interface. Initially this is for raising message events but could be used for 
    /// exception handling and retrying etc.
    /// </summary>
    public class ControllerDecorator : IProjectorController
    {
        private readonly IProjectorController controller;

        public ControllerDecorator(IProjectorController controller)
        {
            this.controller = controller;

            // wire up the controller event to rebroadcast through the decorator
            // for controller specific messages
            controller.Message += (sender, s) =>
                { this.Message.Invoke(sender, s); };
        }

        public event EventHandler<string> Message;

        public string Make
        {
            get
            {
                return controller.Make;
            }
            set
            {
                controller.Make = value;
            }
        }

        public string Model
        {
            get
            {
                return controller.Model;
            }
            set
            {
                controller.Model = value;
            }
        }

        public void PowerOn()
        {
            this.Message.Invoke(controller, "Starting Power On");
            controller.PowerOn();
            this.Message.Invoke(controller, "Power On Complete");
        }

        public void PowerOff()
        {
            this.Message.Invoke(controller, "Starting");
            controller.PowerOff();
            this.Message.Invoke(controller, "Complete");
        }

        public void ProjectorSelectRGB()
        {
            this.Message.Invoke(controller, "Starting");
            this.ProjectorSelectRGB();
            this.Message.Invoke(controller, "Complete");
        }

        public void ProjectorSelectHDMI()
        {
            this.Message.Invoke(controller, "Starting");
            this.ProjectorSelectHDMI();
            this.Message.Invoke(controller, "Complete");
        }

        public void ProjectorFreeze()
        {
            this.Message.Invoke(controller, "Starting");
            this.ProjectorFreeze();
            this.Message.Invoke(controller, "Complete");
        }

        public void ProjectorAuto()
        {
            this.Message.Invoke(controller, "Starting");
            this.ProjectorAuto();
            this.Message.Invoke(controller, "Complete");
        }

        public void IsMe()
        {
            controller.IsMe();            
        }

        public void ProjectorSelectRGB2()
        {
            this.Message.Invoke(controller, "Starting");
            this.ProjectorSelectRGB2();
            this.Message.Invoke(controller, "Complete");
        }

        public void ProjectorSelectVideo()
        {
            this.Message.Invoke(controller, "Starting");
            this.ProjectorSelectVideo();
            this.Message.Invoke(controller, "Complete");
        }

        public void ProjectorSelectSVideo()
        {
            this.Message.Invoke(controller, "Starting");
            this.ProjectorSelectSVideo();
            this.Message.Invoke(controller, "Complete");
        }

        public void ProjectorSelectComponent()
        {
            this.Message.Invoke(controller, "Starting");
            this.ProjectorSelectComponent();
            this.Message.Invoke(controller, "Complete");
        }
    }
}