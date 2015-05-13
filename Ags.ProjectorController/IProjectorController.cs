using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ags.ProjectorController
{
    using System;

    public interface IProjectorController
    {
        /// <summary>
        /// This is an event that can be raised anywhere in this controller that will 
        /// broadcast a message
        /// </summary>
        event EventHandler<string> Message;
        
        string Make { get; set; }
        string Model { get; set; }
        void PowerOn();
        void PowerOff();
        void ProjectorSelectRGB();
        void ProjectorSelectHDMI();
        void ProjectorFreeze();
        
        void ProjectorAuto();
        void IsMe();
        void ProjectorSelectRGB2();

        void ProjectorSelectVideo();

        void ProjectorSelectSVideo();

        void ProjectorSelectComponent();
    }
}
