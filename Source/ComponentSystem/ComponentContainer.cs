using Godot;
using System;
using System.Linq;

namespace Vain
{
	

	public class ComponentContainer : Node
		
	{


		public Component[] Components => GetChildren().OfType<Component>().ToArray();

		public T GetComponent<T>(bool nullable = false) where T : Component
		{	
			var resArray = GetChildren().OfType<T>().ToArray();

			if(resArray.Count() == 0 && nullable)
				return null;
			


			return resArray.First();
		}

		

	}
}
