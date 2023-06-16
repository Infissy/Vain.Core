using System.Reflection;
using System.Linq;
using Godot;

namespace Vain
{   

    public partial class Component : Node
    {
        
        
        public Character Character { get; private set; }



        
        public override void _Ready()
        {
            base._Ready();


            if(!Engine.IsEditorHint())
                Character = GetParent<Character>();
            else
            {
                base.UpdateConfigurationWarnings();
            }



        
        }

        /*
        //ToolAttribute isn't inherited, as such there's no way for a component to handle warnings without using ToolAttribute in the component itself. 
        //This can lead to repetition, and unecessary behaviour since a component shouldn't be a tool, only the base class.
        //Find a better way to handle.

        public override string[] _GetConfigurationWarnings()
        {
            
    
            var fields = this.GetType().GetFields();
            foreach (var field in fields)
            {
                var componentAttribute = field.GetCustomAttribute<RequiredComponentAttribute>();
                if(componentAttribute != null)
                {
                    if(!GetParent().GetChildren().Any(c => c.GetType() == field.FieldType))
                    {
                        return new string[]{$"Component ${this.GetType()} requires a ${field.FieldType} Component."};
                    }

                }

                var childAttribute = field.GetCustomAttribute<RequiredChildAttribute>();
                if(childAttribute != null)
                {
                    if(!GetChildren().Any(c => c.GetType() == field.FieldType))
                    {
                        return new string[]{$"Component ${this.GetType()} requires a ${field.FieldType} child.",childAttribute.Message};
                    }

                }
            }
            
        

            return new string[]{};
        }

        */
        public T GetComponent<T>(bool optional = false) where T : Component
        {

            

            //In case we want a component before _ready has been called
            if(Character == null)
                Character = GetParent<Character>();
            
            return Character.GetComponent<T>(optional);


            
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            
        }
    }
}