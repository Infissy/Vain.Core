using System;
using System.Collections;
using System.Collections.Generic;



using Godot;
using Vain.SpellSystem.Elemental.GPU;

namespace Vain.SpellSystem.Elemental
{

   
    internal struct ElementalParticle 
    {


        
        public int ID = 0;

        public Vector3 Position = Vector3.Zero;
        public Vector3 Velocity = Vector3.Zero;

        /// <summary>
        /// Size of the particle, defaults to -1 for uninitialized particle; 
        /// </summary>
        
        public float Radius = -1;


        public float Lifetime  = -1;
        public float Temperature = 0;

        public float Elasticity = 0;

        public ElementalParticle()
        {

        }

     

        public static ElementalParticle ParticleToParticleInteraction(ElementalParticle particle, ElementalParticle other, double delta)
        {
            if(particle.ID == other.ID)
                return particle;
            var deltapos = other.Position - particle.Position;
            var distance = (deltapos).Length(); 
            var normal = deltapos / distance;
            if( distance < particle.Radius + other.Radius)
            {
                particle.Velocity += normal * (distance / (particle.Radius + other.Radius)  / Mathf.Max(particle.Elasticity,0.001f)) * (float)delta;


            }


            
            return particle;
        }

        Vector3 particleToParticleCostrains(ElementalParticle particle, ElementalParticleResource resource, ElementalParticle other,ElementalParticleResource otherResource, double delta)
        {
            if(particle.ID == other.ID)
                return Vector3.Zero;
            var res = Vector3.Zero;
   
            var distance = particle.Position.DistanceTo(other.Position);
            var normal = (other.Position - particle.Position).Normalized(); 
            if( distance < particle.Radius + other.Radius && resource.Elasticity == 0)
            {
          
                res =  normal * (distance -  particle.Radius + other.Radius); 
             

            }

            return res;
        }

    
            

    
    }
}