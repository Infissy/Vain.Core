#if TOOLS

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

        readonly static string _CharacterFolderPath = ProjectConfig.LoadConfiguration(ProjectConfig.SingleSourceConfiguration.CharacterFolder);
        const string _CharacterPrefab = "res://addons/interactiongraph/Nodes/CharacterNode.tscn";

        public static void GenerateCharacters()
        {
            var npcDataList  = new List<CharacterInfo>();


            var dir = DirAccess.Open(_CharacterFolderPath);

            var fileNames = dir.GetFiles();
            
            foreach (var filename in fileNames)
            {
                
                var file = ResourceLoader.Load(_CharacterFolderPath + "/" + filename);

                if(file is PackedScene scene)
                {
                    npcDataList.Add(scene.Instantiate<CharacterInfo>());
                }

                var npc = ResourceLoader.Load<PackedScene>(_CharacterPrefab).Instantiate<CharacterNode>();


                npc.SetCharacters(npcDataList.Select(npc => npc.Name).ToList());
            
                var packedScene = new PackedScene();
                packedScene.Pack(npc);
                
                ResourceSaver.Save(packedScene,_CharacterPrefab);
            }
        }
        

        public static void GenerateInteractions()
        {

           var interactionTypes = Assembly
                .GetAssembly(typeof(Interaction)).GetTypes()
                .Where(
                    type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Interaction))
                    );

            
            foreach (var interaction in interactionTypes)
            {
                InteractionNode.CreateTemplate(interaction);
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
#endif