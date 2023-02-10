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

           var interactionTypes = Assembly
                .GetAssembly(typeof(Interaction)).GetTypes()
                .Where(
                    type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Interaction))
                    );

            




            
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