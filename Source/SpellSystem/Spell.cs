using System;
using Godot;
using Vain.Configuration;
using Vain.Core;
using Vain.Core.ComponentSystem;
using Vain.HubSystem;
using Vain.SpellSystem.Aspects;
using static Vain.HubSystem.GameEvent.GameEvents.Entity;
using static Vain.HubSystem.GameEvent.GameEvents.Spell;

namespace Vain.SpellSystem;


public enum SpellInput {
    EnterCast,
    Top,
    Down,
    Left,
    Right,
    ExitCast,
}



//Rules :
//All behaviour must be defined before instantiation (noo parameters will after it (for now))
//Every aspect will be defined by a previous aspect, and might have successive aspects (no aspect chosen can change already chosen aspects)

public partial class Spell : Area2D, IEntity
{
    
    public uint RuntimeID { get; set; }
    public Character Caster { get; set; }
    
    internal Aspect Aspect { get; private set;}

    
    SpellUpdateSpecification _updateSpecs = new SpellUpdateSpecification();
    ColorRect _rect;
    
    ShaderMaterial _spellShader;

    private Spell() {}
    internal Spell(Character caster, Aspect aspect, Vector2 target){
        Caster = caster;
        Initialize(aspect, caster, target);

    }

    public override void _Ready(){
        base._Ready();

        _rect = new ColorRect
        {
            Color = Colors.White,
            Size = new Vector2(10,10),
        };

        _spellShader = new ShaderMaterial()
        {
            Shader = ResourceLoader.Load<Shader>(ProjectConfiguration.LoadConfiguration("Shaders","SpellShader")[0])
        };
        _rect.Material = _spellShader;

        _spellShader.SetShaderParameter("RADIUS", _updateSpecs.Size);
        _spellShader.SetShaderParameter("SPELL_COLOR", _updateSpecs.Color);



        

        AddChild(_rect);


        this.BodyEntered += OnBodyCollision;
        this.AreaEntered += OnAreaCollision;

        Hub.Instance.Emit<EntityInstantiatedEvent, EntityArgs>(new EntityArgs{Entity = this});
    }
    internal void Initialize(Aspect aspect, Character caster, Vector2 target) {
        
        Aspect = aspect;

        var initSpecs = new SpellInitializationSpecification(caster.GlobalPosition, target);
        Aspect.Initialize(ref initSpecs, ref _updateSpecs);


    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if(_updateSpecs.Penetration == 0)
        {
            this.DisableMode = DisableModeEnum.Remove;
            this.QueueFree();
            return;
        } 

        Aspect.Process(ref _updateSpecs, delta);

      
        this.GlobalTranslate(_updateSpecs.direction * _updateSpecs.Velocity * (float)delta);
        _spellShader.SetShaderParameter("SPELL_COLOR", _updateSpecs.Color);
        _spellShader.SetShaderParameter("RADIUS", _updateSpecs.Size);

    }




    public override void _ExitTree()
    {
        base._ExitTree();
        
        Hub.Instance.Emit<EntityDestroyedEvent, EntityArgs>(new EntityArgs{Entity = this});
    }



    void OnBodyCollision (Node2D body){

        if(!(body is Character c && c != Caster))
            return;

        var character = body as Character;
        
        Hub.Instance.Emit<SpellCharacterHitEvent,SpellCharacterHitEventArgs>(new SpellCharacterHitEventArgs{Character = character, Spell = this});
        
        var hitSpecs = new SpellHitSpecification(HitType.Character); 

        Aspect.Hit(ref hitSpecs, ref _updateSpecs);
        
        
            
        

    }
    void OnAreaCollision(Area2D area){
        if(area is not Spell) 
            return;

        var spell = area as Spell;
        
        
        Hub.Instance.Emit<SpellCollisionEvent,SpellCollisionEventArgs>(new SpellCollisionEventArgs{Caller = this , Collided = spell});

        var hitSpecs = new SpellHitSpecification(HitType.Spell);
        Aspect.Hit(ref hitSpecs,ref _updateSpecs);
    
    }

}

