using System;
using System.Threading.Tasks;
using System.Linq;
using Godot;
using Vain.SpellSystem.Elemental;
namespace Vain.Plugins.VainUtility.ElementalMeshGeneration
{
    internal partial class MeshGenerationButton : Module
    {
        //FIXME: Find a better solution to export configuration values
        const string RESOURCES_PATH = "res://Resources/3D/SpellSystem/MeshResources/";

        Button _button;
        public override void _EnterTree()
        {
            _button = new Button();
            _button.Text = "Generate Meshes";
            
            LayoutMode = 1;
            _button.AnchorsPreset = (int)Control.LayoutPreset.HcenterWide;


            _button.Pressed += generateMeshes;
            AddChild(_button);
           
        }
        void addPercentage(int percentage)
        {
            if(!_button.Text.Contains("%"))
            {
                _button.Text = "0%";
                _button.Disabled = true;
            }
            var totalPercentage = Convert.ToInt32(_button.Text.Replace("%",string.Empty)) + percentage;
            _button.Text =  totalPercentage + "%";

            if(totalPercentage >= 100)
            {
                _button.Text = "Generate Meshes";
                _button.Disabled = false;
            }
            
        }
        internal override string GetPluginName()
        {
            return "ElementalMeshGeneration";
        }







        void generateMeshes()
        {
            //addPercentage(0);
            /*
            var names = DirAccess.GetFilesAt(RESOURCES_PATH);
            var tasks = names.Select(name => ResourceLoader.Load<ElementalMeshResource>(RESOURCES_PATH + name)).Select(mesh => new Task(() => mesh.GenerateMesh()));
            

            tasks.Select(task => task.ContinueWith( (_) => addPercentage(100/names.Count())));
            

            Task.WhenAll(tasks).ContinueWith((_) => addPercentage(100));
            */
            var names = DirAccess.GetFilesAt(RESOURCES_PATH);
            var mesh =ResourceLoader.Load<ElementalMeshResource>(RESOURCES_PATH + names[0]);
            mesh.GenerateMesh();
        }
    }
}