using Godot;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Vain.SpellSystem.Elemental
{
    //Node that rappresents the elemental spell cluster
    internal partial class ElementalSystem : Node
    {
        class BufferWrapper
        {
            ElementalParticleBuffer Buffer;
        }

        [Export]
        ElementalSpell _test;

        [Export]
        PhysicsSystem _physicsSystem;


        [Export]
        public ElementalParticleBuffer Buffer {get; private set;} = new ElementalParticleBuffer();



        Task _physicsTask;
        double _physicsDelta = 0;
        
        public override void _Ready()
        {
            base._Ready();
            _physicsSystem.Buffer = Buffer;


            AddSpell(_test,Vector3.Zero);
            //SetPhysicsProcess(false);
            _physicsSystem.Running = true;
        }

      



        public override void _PhysicsProcess(double delta)
        {
            _physicsSystem.Delta = delta;

        }
      
        public void AddSpell(ElementalSpell spell, Vector3 target)
        {
            Buffer.AddCluster(spell.Prefab.Particle,spell.Prefab.GetCluster());
        
        }

    }
}