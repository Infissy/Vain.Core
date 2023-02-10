using System;
using Godot;

namespace Vain.InteractionSystem.InteractionGraph
{
    [Tool]
    internal abstract partial class NodeField<T>: HBoxContainer 
    {

        string _name;
        
        


        internal string FieldName 
        {
            get => GetChild<Label>(0).Text;
            set
            {
                var label = GetChildOrNull<Label>(0);
                if(label != null)
                {

                    label.Name = value.Trim();
                    label.Text = value;
                }
                else
                {
                    _name = value;
                }
            }



        }
        
        

        public override void _Ready()
        {
        

            base._Ready();

        
            if(GetChildOrNull<Label>(0) == null)
            {
                
                var label = new Label();

                label.Text = _name;

                AddChild(label);
            }

          
        }




        



    }

}
