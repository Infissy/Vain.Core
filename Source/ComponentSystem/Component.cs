using System;
using System.Collections.Generic;
using Godot;
namespace Vain
{




	
	public abstract class Component 
	{
		
		public Entity Entity {get;set;}
	

		

		protected T GetComponent<T>() where T : Component
		{
			return Entity.GetComponent<T>();
		}
		
	}












	
}