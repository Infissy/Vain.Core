#if TOOLS
using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;

namespace Vain.InteractionSystem.InteractionGraph
{

    partial class InteractionNode : GraphNode
    {
        
        const string INTERACTION_FOLDER_PATH = "res://addons/interactiongraph/Nodes/InteractionsTemplate/";
        
        
        

        List<Node> _fields = new List<Node>();
        

        Interaction Interaction {get;set;}
        


        static public InteractionNode CreateTemplate(Type interaction)
        {

            
            var interactionNode = new InteractionNode();

            interactionNode.Name = interaction.Name;
            interactionNode.Title = interaction.Name;
            
            foreach(var interactionField in interaction.GetProperties())
            {
                if(interactionField.GetCustomAttribute<ExportAttribute>() != null)
                {
                    
                    
                    var type = interactionField.PropertyType;
                    
                    var field = new NodeField(type, interactionField.Name); 
                    
                    interactionNode.AddChild(field);

                    
                    field.Owner = interactionNode;
                    field._Ready();


                    
                    var slotIndex = interactionNode.GetChildCount()-1;



                    //Defaults to true, all internal fields then are set to false;
                    interactionNode.SetSlotEnabledLeft(slotIndex,true);




                    if(type.IsPrimitive)
                    {
                        interactionNode.SetSlotEnabledLeft(slotIndex,false);
                    }

                }


            }

            interactionNode.SetSlotEnabledRight(0,true);


            var scene = new PackedScene();
            scene.Pack(interactionNode);
            var res = ResourceSaver.Save(scene, INTERACTION_FOLDER_PATH + $"{interactionNode.Name}.tscn");

            return interactionNode;

        }


    }
}
#endif