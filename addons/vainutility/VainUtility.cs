#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;

namespace Vain.Plugins.VainUtility
{

	[Tool]
	internal partial class VainUtility : EditorPlugin
	{
		MainDock _dock;
		Dictionary<string,SubDock> _subDocks = new();
		
		public override void _EnterTree()
		{

            _dock = new MainDock
            {
                Name = "VainUtility"
            };
            AddControlToDock(DockSlot.LeftUl,_dock);
			
			
			SpawnModules();
		}

		public override void _ExitTree()
		{
			RemoveControlFromDocks(_dock);
			_dock.QueueFree();
		}


		void SpawnModules()
		{
			var moduleType = typeof(Module);
			Type[] allTypes = moduleType.Assembly.GetTypes();
			

			
			var modules = allTypes.Where(
				t => moduleType.IsAssignableFrom(t) && t != moduleType && !t.IsAbstract).ToArray();



			foreach (var module in modules)
			{
				

				//Obsolete modules are not loaded
				if(module.GetCustomAttribute<ObsoleteAttribute>() != null)
					continue;

				



				var moduleInstance = Activator.CreateInstance(module) as Module;
				var name = moduleInstance.GetPluginName();
				var added = _subDocks.ContainsKey(name);
				
				if(!added)
				{
					_subDocks.Add(moduleInstance.GetPluginName(),new SubDock());
					_dock.AddChild(_subDocks[name]);
				}
				
				
				_subDocks[name].AddToModule(moduleInstance);
				
				

			}
			

		}
	
	
	}




}

#endif
