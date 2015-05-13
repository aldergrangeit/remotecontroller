using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ags.ProjectorController
{
    public interface IProjectorController
    {
        string Make { get; set; }
        string Model { get; set; }
        void PowerOn();
        void PowerOff();
        void ProjectorSelectRGB();
        void ProjectorSelectHDMI();
        void ProjectorFreeze();
        void IsMe();
    }
}
