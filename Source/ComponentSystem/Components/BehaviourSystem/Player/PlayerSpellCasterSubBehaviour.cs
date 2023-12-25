using Godot;
using Vain.Singleton;
using Vain.SpellSystem;

namespace Vain.Core.ComponentSystem.Behaviour;

partial class PlayerSpellCasterSubBehaviour : SubBehaviour
{
    public const string SLOT1 = "player_cast_slot1";
    public const string SLOT2 = "player_cast_slot2";
    public const string SLOT3 = "player_cast_slot3";
    public const string SLOT4 = "player_cast_slot4";
    public const string SLOT5 = "player_cast_slot5";
    public const string SLOT6 = "player_cast_slot6";
    public const string SLOT7 = "player_cast_slot7";
    public const string SLOT8 = "player_cast_slot8";

    public override void _UnhandledInput(InputEvent input)
    {
        if(input is not InputEventAction action)
            return;

        if(!action.Action.ToString().Contains("player_cast_slot"))
            return;
        var spellCaster = BehaviourComponent.Character.GetComponent<SpellCaster>();
        var camera = SingletonManager.GetSingleton<MainCamera>(SingletonManager.Singletons.MAIN_CAMERA);

        switch (action.Action)
        {
            case SLOT1:
                spellCaster.CastSpell(0,camera.Reference.GetMouseScenePosition());
                break;
            case SLOT2:
                spellCaster.CastSpell(1,camera.Reference.GetMouseScenePosition());
                break;
            case SLOT3:
                spellCaster.CastSpell(2,camera.Reference.GetMouseScenePosition());
                break;
            case SLOT4:
                spellCaster.CastSpell(3,camera.Reference.GetMouseScenePosition());
                break;
            case SLOT5:
                spellCaster.CastSpell(4,camera.Reference.GetMouseScenePosition());
                break;
            case SLOT6:
                spellCaster.CastSpell(5,camera.Reference.GetMouseScenePosition());
                break;
            case SLOT7:
                spellCaster.CastSpell(6,camera.Reference.GetMouseScenePosition());
                break;
            case SLOT8:
                spellCaster.CastSpell(7,camera.Reference.GetMouseScenePosition());
                break;
        }
    }
}
