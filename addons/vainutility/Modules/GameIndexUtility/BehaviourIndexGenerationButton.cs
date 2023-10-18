#if TOOLS
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;
using Vain.Core;
using Vain.Core.ComponentSystem;

namespace Vain.Plugins.VainUtility.GameIndex
{
    internal partial class BehaviourIndexGenerationButton : ResourceSelectionModule
    {


        readonly string _IndexPath  = ProjectConfig.LoadConfiguration(ProjectConfig.SingleSourceConfiguration.BehaviourIndex);
  
        public override void _Ready()
        {
            base._Ready();
            ButtonName = "Generate Behaviour Index";
            base.FileSelected += LoadBehaviours;
        }
        internal override string GetPluginName()
        {
            return "GameIndexUtility";
        }
        void LoadBehaviours(Resource resource)
        {
            var subBehaviourMap = new Dictionary<string,GodotObject>();
            
         
            
            string name;
            SubBehaviour subBehaviour = null;
            if(resource is CSharpScript csScript)
            {
                var instance = csScript.New().Obj;
                
                if(instance is not SubBehaviour b)
                {
                    GD.PrintErr("Selected resource is not of SubBehaviour type");
                    return;
                }
                subBehaviour = b;
            }
            else if(resource is PackedScene packedScene)
            {
                var script = packedScene.Instantiate().GetScript();
                var instance = (script.Obj as CSharpScript).New().Obj;
                if(instance is not SubBehaviour b)
                {
                    GD.PrintErr("Selected resource is not of SubBehaviour type");
                    return;
                }
                subBehaviour = b;
            }

            name = subBehaviour.GetType().Name.Replace("SubBehaviour",""); 

            name = string.Concat(name.Replace("AI",":").Replace("NPC",";").Select((x, i) => i > 0 && (char.IsUpper(x) || x == ':' || x == ';')  ? "_" + x.ToString() : x.ToString())).ToLower().Replace(":","ai").Replace(";","npc");

            
            var index = ResourceLoader.Load<IndexResource>(_IndexPath);
            index.IndexedEntities[name] = new IndexedResourceWrapper{ Resource = resource};
            index.EmitChanged();


            GD.Print("Behaviour successfully added to BehaviourIndex!");
        }
    }
}
#endif