using Godot;

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Vain;



namespace Vain.InteractionSystem.InteractionGraph
{


    internal class NodeFactory 
    {

        static string NPC_DIR = "res://Resources/Game/NPCs";
        static string NPC_NODE = "res://addons/interactiongraph/Nodes/CharacterNode.tscn";

        static string INTRACTION_NODE = "res://addons/interactiongraph/Nodes/Interaction.tscn";
        public static void GenerateCharacters()
        {
            List<NPCData> npcDataList  = new List<NPCData>();


            var dir = DirAccess.Open(NPC_DIR);

            var fileNames = dir.GetFiles();
            
            foreach (var filename in fileNames)
            {
                
                var file = ResourceLoader.Load(NPC_DIR + "/" + filename);

                if(file is NPCData data)
                    npcDataList.Add(data);

                var npc = ResourceLoader.Load<PackedScene>(NPC_NODE).Instantiate<CharacterNode>();


                npc.SetCharacters(npcDataList.Select(npc => npc.Name).ToList());
            
                var packedScene = new PackedScene();
                packedScene.Pack(npc);
                
                ResourceSaver.Save(packedScene,NPC_NODE);
            }
        }
        

        public static void GenerateInteractions()
        {

            var interactionNodesDictionary = new Dictionary<Type,List<FieldInfo>>();

            


            var interactionTypes = Assembly
                .GetAssembly(typeof(Interaction)).GetTypes()
                .Where(
                    type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Interaction))
                    );



            foreach (var interactionType in interactionTypes)
            {


                var fields = new List<FieldInfo>();


                
                foreach(var field in interactionType.GetFields())
                {
                    if(field.GetCustomAttribute<ExportAttribute>() != null)
                    {
                        fields.Add(field);
                    }


                }
                if(fields.Count > 0)
                    interactionNodesDictionary.Add(interactionType,fields);



                            
            }


            var interactionTemplate = ResourceLoader.Load<PackedScene>(INTRACTION_NODE).Instantiate<GraphNode>();


            foreach (var interactionNodeData in interactionNodesDictionary)
            {
                var nodeBuilder = InteractionNodeBuilder.CreateNode(interactionTemplate, interactionNodeData.Key.Name);

                foreach (var field in interactionNodeData.Value)
                {
                    
                    if(field.GetCustomAttribute<ExportAttribute>() != null)
                    {
                        var fieldType = field.FieldType;


                        var fieldAdded = false;

                        if(isNumericType(fieldType))
                        {
                            nodeBuilder.AddNumericField(field.Name);
                        }

                        if(!fieldAdded && fieldType.GetType() == typeof(Interaction))
                        {
                            
                        }

                        
                    }
                }

            }








            
        }


        static bool isNumericType(Type type)
        {
            
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Int32:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;     
            }
            
        }
        
    }
}