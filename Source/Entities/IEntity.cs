using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;

namespace Vain.Core;

//Rapresents every single tangible entity in Vain
//Since entities can be of different types (Chracters/Spells/Spelldrop/etc) there is no way to have a single parent class since every entity inherits from a different node.
//So every entity has to implement this interface to be accessed in the command sistem and more abstract stuff that will be implemented later.


public interface IEntity
{
    /// <summary>
    /// At the moment unused, will have some backend implementation in future versions
    /// </summary>


    uint RuntimeID { get; set; }



    public static IReadOnlyDictionary<string, string> EntityPrefabs => new Dictionary<string, string>()
    {
        {"main_camera", "res://Vain.Core/Prefabs/Entities/MainCamera.tscn"},
    };
}

