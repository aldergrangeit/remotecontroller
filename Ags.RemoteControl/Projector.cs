namespace Ags.RemoteControl
{
    using Ags.ProjectorController;

    public class Projector
    {
        public Projector(IProjectorController controller)
        {
            this.Controller = controller;
        }

        public string DisplayName
        {
            get
            {
                return string.Format("{0} ({1})", this.Controller.Make, this.Controller.Model);
            }
        }

        public IProjectorController Controller { get; set; }
    }
}