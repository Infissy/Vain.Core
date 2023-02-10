using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;

namespace Vain.InteractionSystem.InteractionGraph
{

    partial class InteractionNode : GraphNode
    {
        
        const string INTERACTION_FOLDER_PATH = "";
        
        
        

        List<NodeField> _fields = new List<NodeField>();
        

        Interaction Interaction {get;set;}
        


        static public PackedScene CreateTemplate(Type interaction)
        {

            
            var interactionNode = new InteractionNode();

            interactionNode.Name = interaction.Name;

            
            foreach(var interactionField in interaction.GetFields())
            {
                if(interactionField.GetCustomAttribute<ExportAttribute>() != null)
                {
                    
                    
                    var type = interactionField.FieldType;
                    

                    
                   var field = new NodeField() 


                }


            }


            var scene = new PackedScene();
            scene.Pack(interactionNode);
            ResourceSaver.Save(scene,INTERACTION_FOLDER_PATH);

            return scene;

        }


    }
}