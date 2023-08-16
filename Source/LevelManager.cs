using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Godot;

using Vain.Singleton;
namespace Vain.Core
{
	//Should handle all the resources inside a single map/level
	
	
	public partial class LevelManager : Node
	{

		//TODO: Refactor indices so it doesn't overflow
		uint _entityIndex = 0;


		List<Character> _characters = new List<Character>();
		Dictionary<uint, IEntity> _entities = new Dictionary<uint, IEntity>();




		public ReadOnlyCollection<Character> Characters => _characters.AsReadOnly();


		public ReadOnlyCollection<IEntity> Entities => _entities.Values.ToList().AsReadOnly();

		public uint Register(Character character)
		{
			
			_characters.Append(character);

			return Register(character as IEntity);

		}
		public uint Register(IEntity entity)
		{
			_entityIndex ++;

			_entities.Add(_entityIndex, entity);

			return _entityIndex;
		}



		public void Free(Character character)
		{
			_characters.Remove(character);

			Free(character as IEntity);
		}
		public void Free(IEntity entity)
		{
			if(_entityIndex < entity.RuntimeID)
				return;

			_entities.Remove(entity.RuntimeID); 

			
		}
		public override void _EnterTree()
		{
			base._EnterTree();
			SingletonManager.Register(SingletonManager.Singletons.LEVEL_MANAGER,this);
		}


	}
}
