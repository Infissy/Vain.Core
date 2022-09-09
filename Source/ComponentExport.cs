using System;
using Godot;
namespace Vain.Godot
{

    [AttributeUsage(AttributeTargets.Field)]
    class ComponentExportAttribute : Attribute
    {
        
        string Components;
        
        
        public ComponentExportAttribute() 
        {
            
            GD.Print(Components);    


        }
    }
}