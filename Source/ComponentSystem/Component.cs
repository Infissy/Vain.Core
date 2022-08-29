using System;
using System.Collections.Generic;
using Godot;
namespace Vain
{
	[Tool]
	public class ComponentChecker : Node
	{
		public override void _Ready()
		{
			GD.Print("Test");
		}
	}



	
	public abstract class Component : Node
	{
		
		Entity _entity;
		protected Entity ComponentEntity => _entity;


		public override void _Ready()
		{
			//TODO: Enable component hierarchy
			_entity = GetParent<ComponentContainer>().GetParent<Entity>();

		}

		protected T GetComponent<T>() where T : Component
		{
			return _entity.GetComponent<T>();
		}
		
	}












	
}