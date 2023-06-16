#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Vain.Plugins.VainUtility
{

	[Tool]
	internal partial class VainUtility : EditorPlugin
	{
		MainDock _dock;
		public override void _EnterTree()
		{

			_dock = new MainDock();
			_dock.Name = "VainUtility";
			AddControlToDock(DockSlot.LeftUl,_dock);
			
			
			spawnModules();
		}

		public override void _ExitTree()
		{
			RemoveControlFromDocks(_dock);
			_dock.QueueFree();
		}


		void spawnModules()
		{
			var moduleType = typeof(Module);
			Type[] allTypes = moduleType.Assembly.GetTypes();
			

			
			var modules = allTypes.Where(
				t => moduleType.IsAssignableFrom(t) && t != moduleType).ToArray();



			foreach (var module in modules)
			{
				
				var moduleInstance = Activator.CreateInstance(module) as Module;
				
				_dock.AddChild(moduleInstance);

			}
			

		}
	}
}

#endif
