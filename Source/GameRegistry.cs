using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Godot;
using Vain.Core.ComponentSystem;
using Vain.SpellSystem.Aspects;


namespace Vain.Core;

public partial class GameRegistry : Node
{
    public static GameRegistry? Instance { get; set; }

    SortedSet<string> _keys = new();

    Dictionary<string, Type> _entities = new();
    Dictionary<string, Type> _components = new();
    Dictionary<string, Type> _subBehaviours = new();
    Dictionary<string, Type> _aspects = new();
    Dictionary<string, string> _levels = new()
    {
        {"test_level", "res://Vain/Prefabs/Levels/TestScene.tscn"}
    };



    public IReadOnlySet<string> Entities => _entities.Keys.ToImmutableHashSet();
    public IReadOnlySet<string> Components => _components.Keys.ToImmutableHashSet();
    public IReadOnlySet<string> SubBehaviours => _subBehaviours.Keys.ToImmutableHashSet();
    public IReadOnlySet<string> Aspects => _aspects.Keys.ToImmutableHashSet();
    public IReadOnlySet<string> Levels => _aspects.Keys.ToImmutableHashSet();


    public override void _EnterTree()
    {
        base._EnterTree();


        var vainAssembly = Assembly.GetAssembly(typeof(GameRegistry));


        foreach (Type type in vainAssembly.GetTypes())
        {
            var indexing = (NodeIndexingAttribute?)type.GetCustomAttribute(typeof(NodeIndexingAttribute));

            if (indexing == null)
                continue;



            Debug.Assert(!_keys.Contains(indexing.Key));
            _keys.Add(indexing.Key);


            if (type.IsAssignableTo(typeof(Component)))
                _components.Add(indexing.Key, type);

            if (type.IsAssignableTo(typeof(IEntity)))
                _entities.Add(indexing.Key, type);

            if (type.IsAssignableTo(typeof(SubBehaviour)))
                _subBehaviours.Add(indexing.Key, type);

            if (type.IsAssignableTo(typeof(Aspect)))
                _aspects.Add(indexing.Key, type);

        }


        foreach (var levelKey in _levels.Keys)
            _keys.Add(levelKey);


    }

    public Node? Instantiate(string key)
    {
        if (_keys.Contains(key))
            return null;

        if (_levels.ContainsKey(key))
            return ResourceLoader.Load<PackedScene>(key).Instantiate();


        Type? baseNodeType = null;


        if (_components.ContainsKey(key))
            baseNodeType = _components[key];

        if (_subBehaviours.ContainsKey(key))
            baseNodeType = _subBehaviours[key];

        if (_aspects.ContainsKey(key))
            baseNodeType = _aspects[key];

        if (_entities.ContainsKey(key))
            baseNodeType = _entities[key];



        // Get the generic type definition
        MethodInfo? method = typeof(BaseNode).GetMethod("Instantiate",
                                    BindingFlags.Public | BindingFlags.Static);

        // Build a method with the specific type argument you're interested in
        method = method?.MakeGenericMethod(baseNodeType);
        // The "null" is because it's a static method
        BaseNode instance = (BaseNode)method.Invoke(null, null);



        return instance;
    }

}
