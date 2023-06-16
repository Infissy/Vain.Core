using Godot;

namespace Vain.Core
{
    partial class Enemy : NPC
    {

		public override void _Ready()
		{
			base._Ready();
			
			HostileTarget = SingletonManager.GetSingleton<Player>() as Character;


			
		}

    }
}