using System;
using System.Collections.Generic;
using Godot;
namespace Vain
{
	

	public abstract class Component : Node
	{
		
		Entity _entity;
		protected Entity ComponentEntity => _entity;


		public override void _Ready()
		{

			_entity = GetParent<ComponentContainer>().GetOwner<Entity>();

		}
		
	}












	
}