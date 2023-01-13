using Godot;
using System;
using System.Linq;

namespace Vain
{
	public partial class NPC : Character
	{
		[Export]
		public float HostilityToPlayer { get; set; }
		[Export]
		public float AggressionLevel { get; set; }


		public override void _Ready()
		{
			base._Ready();

			if(HostilityToPlayer > AggressionLevel)
			{
				//TODO: maybe fix in the future, for now a character is able by default to be aggressive only to the player


				var playableGame = GetNode<PlayableGame>("/root/Game/PlayableGame");

				foreach(var child in GetChildren())
				{
					if (child is HostileTargetBehavior hostileHandlerComponent)
						hostileHandlerComponent.HostileTarget = playableGame.PlayableCharacter;
				}
				
			}
		}

	}
	
}
