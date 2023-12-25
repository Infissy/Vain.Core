using System;
using System.Collections.Generic;


using Godot;

using Vain.Core;

namespace Vain.Singleton;

    /// <summary>
    /// Manages singletons in the whole game.
    /// Currently it can't delete singletons, and the only way to add one is through SingletonManager.Register(this) in _EnterTree of the script needed;
    /// Look into better ways, eventually with attributes.
    /// </summary>

public partial class SingletonManager : Node
{


    static readonly Dictionary<string, SingletonHandle> _singletons = new();
    static readonly Dictionary<string, List<Action>> _initializationCallbacks = new();




    public static SingletonCharacter GetCharacterSingleton(string key, Action callback = null)
    {
        return GetSingleton<Character>(key, callback) as SingletonCharacter;
    }



    public static Singleton<T> GetSingleton<T>(string key, Action callback = null) where T : Node
    {

        if (_singletons.ContainsKey(key))
            return _singletons[key] as Singleton<T>;

        if (!_initializationCallbacks.ContainsKey(key))
            _initializationCallbacks.Add(key, new List<Action>());
        _initializationCallbacks[key].Add(callback);


        _singletons[key] = new Singleton<T>(null);
        return _singletons[key] as Singleton<T>;
    }



    public static void Register<T>(string key, T instance) where T : Node
    {
        if (_singletons.ContainsKey(key))
        {
            var singleton = _singletons[key] as Singleton<T>;
            singleton.Reference = instance;
            return;
        }


        SingletonHandle singletonHandle = instance is Character character ? new SingletonCharacter(character) : new Singleton<T>(instance);
        _singletons.Add(key, singletonHandle);

        if (!_initializationCallbacks.ContainsKey(key))
            return;


        foreach (var callback in _initializationCallbacks[key])
        {
            callback.Invoke();
        }
        _initializationCallbacks.Remove(key);

    }


    public static IEnumerable<string> GetSingletonsList()
    {
        return _singletons.Keys;
    }


    public static void Destroy(string key)
    {
        _singletons[key].Disposed = true;

        _singletons.Remove(key);

    }




    public static class Singletons
    {

        public static class UI
        {
            public const string SPELL_BAR = "ui_spell_bar";
            public const string HEALTH_BAR = "ui_health_bar";
            public const string DEBUG_OVERLAY = "ui_debug_overlay";
        }
        public const string MAIN_CAMERA = "main_camera";
        public const string LEVEL_MANAGER = "level_manager";
        public const string PLAYER = "entity_player";
        public const string INTERACTION_HANDLER = "interaction_handler";
        public const string ENEMY_SPAWNER = "enemy_spawner";
        public const string GAME_REGISTRY = "game_registry";
    }

}

