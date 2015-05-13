using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Ags.ProjectorController;

namespace prm35serialcommands
{
    class Program
    {
        static void Main(string[] args)
        {

            var listArgs = args.ToList();

            var port = listArgs.First();

            port = port.Replace("/", "").ToUpper();

            listArgs.RemoveAt(0);

            var controllerType = listArgs.First();

            listArgs.RemoveAt(0);

            IProjectorController controller = null;

            switch (controllerType)
            {
                case "Promer":
                    controller = new PrometheonPrm35Controller(port);
                    break;
                default:
                    throw new ApplicationException("NO CONTROLLER TYPE");
            }
            
            try
            {
                foreach (string s in args)
                {
                    switch (s)
                    {
                        case "/PowerOn":
                            controller.PowerOn();
                            break;
                        case "/PowerOff":
                            controller.PowerOff();
                            break;
                        case "/ProjectorAuto":
                            controller.ProjectorAuto();
                            break;
                        case "/ProjectorSelectRGB":
                            controller.ProjectorSelectRGB();
                            break;
                        case "/ProjectorSelectRGB2":
                            controller.ProjectorSelectRGB2();
                            break;
                        case "/ProjectorSelectVideo":
                            controller.ProjectorSelectVideo();
                            break;
                        case "/ProjectorSelectSVideo":
                            controller.ProjectorSelectSVideo();
                            break;
                        case "/ProjectorSelectComponent":
                            controller.ProjectorSelectComponent();
                            break;
                        case "/ProjectorSelectHDMI":
                            controller.ProjectorSelectHDMI();
                            break;
                        case "/?":
                            Console.Write("The Commands that are currently available are as follows:\n");
                            Console.Write("/PowerOn - This Command will switch on the projector\n");
                            Console.Write("/PowerOff - This Command will switch off the projector\n");
                            Console.Write("/ProjectorAuto - This Command will tell the projector to adjust the image automatically\n");
                            Console.Write("/ProjectorSelectRGB - This command will tell the projector to Select RGB 1 as its current display\n");
                            Console.Write("/ProjectorSelectRGB2 - This command will tell the projector to Select RGB 2 as its current display\n");
                            Console.Write("/ProjectorSelectVideo - This command will tell the projector to select Video as its current display\n");
                            Console.Write("/ProjectorSelectSVideo - This command will tell the projector to select S Video as its current display\n");
                            Console.Write("/ProjectorSelectComponent - This command will tell the projector to select component as its current display\n");
                            Console.Write("/ProjectorSelectHDMI - This command will tell the projector to select HDMI as its current display\n");
                            break;

                        default:
                            Console.Write("You have not stated a fuction or i do not reconise the fuction you have stated please type /? for help on surported functions");
                            break;
                    }

                }
            }
            catch(NotImplementedException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.Write("I cannot open COM Port 1");
                Console.Write(ex.ToString());
            }
        }
    }
}
