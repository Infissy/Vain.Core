using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vain
{
	

	public class ComponentContainer 
	{


		public List<Component> _components = new List<Component>();
			
		public Component[] Components => _components.ToArray(); 

		
		public T GetComponent<T>(bool nullable = false) where T : Component
		{	
			var resArray =  _components.Where(c => c.GetType() == typeof(T)).FirstOrDefault();



			
			if(nullable)
				return resArray as T;
			else
			{
				return resArray as T ?? throw new ComponentNotFoundException<T>();
			}

		}

		public void AddComponent(Component component)
		{
			_components.Add(component);
		}
		

	}
}
