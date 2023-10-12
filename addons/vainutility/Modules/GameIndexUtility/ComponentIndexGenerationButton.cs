#if TOOLS
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Vain.Core.ComponentSystem;
using Vain.Singleton;
using Vain.Core;

namespace Vain.Plugins.VainUtility.GameIndex
{

    internal partial class ComponentIndexGenerationButton : ButtonModule
    {
        const string CLASS_INDEX_PATH = GameData.Indices.ClassIndex;
        const string COMPONENT_INDEX_PATH = GameData.Indices.ComponentIndex;
        const string COMPONENT_SCENE_FOLDER_PATH = GameData.Folders.ComponentFolder;
     
        

        public override void _Ready()
        {
            base._Ready();
            ButtonName = "Generate Component Index";
            Pressed += LoadComponents;

        }
        internal override string GetPluginName()
        {
            return "GameIndexUtility";
        }
        void LoadComponents()
        {
            var componentMap = new Dictionary<string,GodotObject>();
            
            var moduleType = typeof(Component);
			Type[] assemblyTypes = moduleType.Assembly.GetTypes();
			

			
			var components = assemblyTypes.Where(t => moduleType.IsAssignableFrom(t) && t != moduleType && !t.IsAbstract);


            var componentScenes = DirAccess.GetFilesAt(COMPONENT_SCENE_FOLDER_PATH).Select( s => ResourceLoader.Load<PackedScene>($"{COMPONENT_SCENE_FOLDER_PATH}/{s}"));
            
        
            var classIndex = ResourceLoader.Load<IndexResource>(CLASS_INDEX_PATH);



            foreach (var component in components)
            {
                var name = component.Name.Replace("Component",""); 
                name = string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
                
                componentMap[name] = classIndex.IndexedEntities[component.Name];
            }
            
            foreach (var scene in componentScenes)
            {
                
                var csscript = scene.Instantiate().GetScript().Obj as CSharpScript;
                var name = csscript.New().Obj.GetType().Name.Replace("Component",""); 

                name = string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
                
                componentMap[name] = scene;

                //! Couldn't find an elegant way to fetch a script from a class/type

            }

            var index = new IndexResource{IndexedEntities =  new Godot.Collections.Dictionary<string, GodotObject>()};
            foreach (var component in componentMap)
                index.IndexedEntities[component.Key] = new IndexedResourceWrapper{Resource = component.Value};
            index.EmitChanged();
            ResourceSaver.Save(index,COMPONENT_INDEX_PATH);

            GD.Print("Component Index Successfuly Generated");
        }


	
    }
}
#endif