using Godot;
using System;
using System.Linq;
using Vain.Core;

namespace Vain
{
	public abstract partial class NPC : Character
	{
		
		[Export]
		public float AggressionLevel { get; set; }

		
		[Export]
		public Character HostileTarget {get;set; }


		
		

		
	}
	
}
