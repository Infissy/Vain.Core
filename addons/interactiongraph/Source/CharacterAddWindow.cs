using Godot;
using System;

using System.Reflection;


namespace Vain.InteractionSystem.InteractionGraph
{
    [Tool]
    public partial class CharacterAddWindow : Window
    {

        public override void _Ready()
        {
            foreach(var c in GetChildren())
                c.QueueFree();


            initialize();
        }



        void initialize()
        {
            var propertyList = WorldNPCInfo.GetGodotPropertyList();

            foreach (var property in propertyList)
            {

                var container = new HBoxContainer(); 


                var label = new Label();
                label.Text = property.Name;
                container.AddChild(label);



                
               
            }


            var button = new Button();
            
           
        }





    }
}
